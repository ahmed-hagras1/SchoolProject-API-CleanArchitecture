using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.BackgroundServices
{
    public class RefreshTokenCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RefreshTokenCleanupService> _logger;

        public RefreshTokenCleanupService(IServiceProvider serviceProvider, ILogger<RefreshTokenCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Refresh Token Cleanup Service is starting.");

            // Set the timer to run once every 24 hours
            var loopDelay = TimeSpan.FromHours(24);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupOldTokensAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while cleaning up old refresh tokens.");
                }

                // Put the worker to sleep for 24 hours. 
                // When it wakes up, the loop runs again!
                await Task.Delay(loopDelay, stoppingToken);
            }
        }

        private async Task CleanupOldTokensAsync()
        {
            // 1. Create a temporary Scope to safely access the database
            using var scope = _serviceProvider.CreateScope();

            // 2. Resolve your repository from this scope
            var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();

            // 3. Define "Safely Dead". Let's say any token that expired more than 14 days ago.
            var deadDateThreshold = DateTime.UtcNow.AddDays(-14);

            // 4. Fetch the dead tokens
            var deadTokens = refreshTokenRepository.GetTableNoTracking()
                .Where(x => x.ExpiryDate < deadDateThreshold)
                .ToList();

            // 5. Delete them if any exist
            if (deadTokens.Any())
            {
                // Note: Assuming your IRefreshTokenRepository has a method to delete a list of entities.
                // If it doesn't, you can loop through them and call DeleteAsync(token).
                await refreshTokenRepository.DeleteRangeAsync(deadTokens);

                _logger.LogInformation($"Successfully purged {deadTokens.Count} dead refresh tokens from the database.");
            }

            // even need to fetch the list into memory first. You can replace steps 4 and 5 with a single, ultra-fast line:
            // await refreshTokenRepository.GetTableNoTracking().Where(x => x.ExpiryDate < deadDateThreshold).ExecuteDeleteAsync();
        }
    }
}
