using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SchoolProject.Service.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly JWTSettings _jwtSettings;
        private readonly IRefreshTokenRepository _RefreshTokenRepository;
        private readonly UserManager<User> _userManager;
        #endregion

        #region Constructor
        public AuthenticationService(IOptions<JWTSettings> jwtSettings,
            IRefreshTokenRepository refreshTokenRepository,
            UserManager<User> userManager)
        {
            _jwtSettings = jwtSettings.Value;
            _RefreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
        }
        #endregion

        #region Methods
        public async Task<JWTAuthResult> GetJWTToken(User user)
        {
            // Generate the JWT token and the access token string, and then save the Jti claim in the database along with the refresh token, so we can use it to validate the refresh token later. 
            var (jwtToken, accessToken) = GenerateJWTToken(user);

            var refreshTokenString = GenerateRefreshToken();

            var jwtAuthResult = new JWTAuthResult()
            {
                AccessToken = accessToken,
                RefreshToken =  new RefreshToken()
                {
                    UserName = user.UserName,
                    TokenString = refreshTokenString,
                    ExpireAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays)
                }
            };

            var userRefreshToken = new UserRefreshToken()
            {
                UserId = user.Id,
                Token = refreshTokenString,
                JWTId = jwtToken.Id, // This grabs the ID from the Jti claim we added above
                IsUsed = false,
                IsRevoked = false,
                AddedTime = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays)
            };

            await _RefreshTokenRepository.AddAsync(userRefreshToken);

            return jwtAuthResult;
        }

        public async Task<JWTAuthResult> GetRefreshToken(string accessToken, string refreshToken)
        {
            // 1. Read token to get claims (This safely validates the signature and ignores expiration)
            var jwtToken = ReadJWTToken(accessToken);

            var jti = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var userName = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.UserName))?.Value;

            if (string.IsNullOrEmpty(jti) || string.IsNullOrEmpty(userName))
            {
                throw new SecurityTokenException("TokenClaimsMissing");
            }

            // 2. Get User from the database
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new SecurityTokenException("UserNotFound");
            }

            // 3. Find the refresh token in the database
            var userRefreshToken = await _RefreshTokenRepository.GetTableNoTracking()
                .FirstOrDefaultAsync(x => x.Token == refreshToken && x.UserId == user.Id && x.JWTId == jti);

            if (userRefreshToken == null)
            {
                throw new SecurityTokenException("RefreshTokenNotFound");
            }

            // 4. NEW: Time Validation
            if (userRefreshToken.ExpiryDate <= DateTime.UtcNow)
            {
                // Change the status to revoked and ensure it is not marked as used
                userRefreshToken.IsRevoked = true;
                userRefreshToken.IsUsed = false;

                // Save the changes to the database
                await _RefreshTokenRepository.UpdateAsync(userRefreshToken);

                // Stop the process and force the user to log in again
                throw new SecurityTokenException("RefreshTokenExpired");
            }

            // 5. Check if it was already used or revoked for other reasons
            if (!userRefreshToken.IsActive)
            {
                throw new SecurityTokenException("RefreshTokenRevoked");
            }

            // 6. Consume the old refresh token so it can never be used again (Prevents Replay Attacks)
            userRefreshToken.IsUsed = true;
            userRefreshToken.IsRevoked = false;
            await _RefreshTokenRepository.UpdateAsync(userRefreshToken);

            // 7. Generate and return the brand new Access Token and Refresh Token pair
            return await GetJWTToken(user);
        }

        // Use Tuple to return both the JWT token object and the access token string, so we can use the JWT token object to get the Jti claim and save it in the database, and use the access token string to send it to the client.
        private (JwtSecurityToken,string) GenerateJWTToken(User user)
        {
            // List of claims that will be included in the token, and these claims will be used to identify the user and his roles, and other information that you want to include in the token.
            // This is built-in Claims.
            //var claims = new List<Claim>()
            //{
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim(ClaimTypes.Email, user.Email)
            //};

            // This is custom Claims, and you can make any claim you want, and you can use it in the future to identify the user and his roles, and other information that you want to include in the token.
            var claims = new List<Claim>
            {
                new Claim(nameof(UserClaimModel.UserName), user.UserName ?? string.Empty),
                new Claim(nameof(UserClaimModel.Email), user.Email ?? string.Empty),
                new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber ?? string.Empty),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Create the security key from your JWT settings
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var jwtToken = new JwtSecurityToken(_jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                null,
                DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature) // Corrected this line
                );

            // Write the token to a string so it can be sent to the client.
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return (jwtToken, accessToken);
        }
        private string GenerateRefreshToken()
        {
            // Create an empty array to hold 32 bytes (256 bits)
            var randomNumber = new byte[32];

            // Fill the array with cryptographically strong random bytes
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            // Convert to Base64 so it can be saved in a database or sent via HTTP
            // Result looks like: "v7x/8E+L4/qR1Z5... (about 44 characters long)"
            return Convert.ToBase64String(randomNumber);
        }
        private JwtSecurityToken ReadJWTToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),

                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuer = _jwtSettings.Issuer,

                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidAudience = _jwtSettings.Audience,

                // CRITICAL FIX: Set to false so we can read the token even if it is expired.
                // We only care that the signature (the cryptography) is valid.
                ValidateLifetime = false
            };
            var jwtToken = tokenHandler.ReadJwtToken(accessToken);

            try
            {
                // This validates the signature. If a hacker tampered with the token, it throws an error here.
                tokenHandler.ValidateToken(accessToken, parameters, out var validatedToken);

                // Ensure the token is a JWT and uses the correct encryption algorithm
                if (validatedToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("AlgorithmIsInvalid");
                }

                return jwtSecurityToken;
            }
            catch
            {
                // Catch any validation errors (like a tampered signature) and throw a clean exception
                throw new SecurityTokenException("TokenIsInvalid");
            }
        }
        public Task<string> ValidateToken(string accessToken)
        {
            // 1. Call our secure, private helper to read the token
            var jwtToken = ReadJWTToken(accessToken);

            // 2. Extract the JTI (JWT ID) claim
            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (string.IsNullOrEmpty(jti))
            {
                throw new SecurityTokenException("JtiClaimMissing");
            }

            // 3. Return the extracted JTI
            return Task.FromResult(jti);
        }
        #endregion
    }
}
