using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrastructure.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Infrastructure.Dependencies
{
    public static class IdentityDependencies
    {
        public static IServiceCollection AddIdentityDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // Bind the JWTSettings class for Dependency Injection (IOptions)
            var jwtSection = configuration.GetSection("JwtSettings");
            services.Configure<JWTSettings>(jwtSection);

            // Extract the settings to configure the JWT Middleware directly
            var jwtSettings = jwtSection.Get<JWTSettings>();

            // Configure Authentication & JWT Bearer validation
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Set to true in Production
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

                    ValidateIssuer = jwtSettings.ValidateIssuer,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidAudience = jwtSettings.Audience,

                    ValidateLifetime = jwtSettings.ValidateLifetime,
                    ClockSkew = TimeSpan.Zero // Removes the default 5-minute grace period on token expiration
                };
            });

            services.AddSingleton<ConcurrentDictionary<string, RefreshToken>>();

            services.AddAuthorization(option =>
            {
                option.AddPolicy("CreateStudent", policy =>
                {
                    policy.RequireClaim("Create Student", "true");
                });

                option.AddPolicy("EditStudent", policy =>
                {
                    policy.RequireClaim("Edit Student", "true");
                });

                option.AddPolicy("DeleteStudent", policy =>
                {
                    policy.RequireClaim("Delete Student", "true");
                });
            });

            return services;
        }
    }
}
