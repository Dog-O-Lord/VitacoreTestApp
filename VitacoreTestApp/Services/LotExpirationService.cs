using Microsoft.EntityFrameworkCore;
using VitacoreTestApp.Data;
using VitacoreTestApp.ViewModels;

namespace VitacoreTestApp.Services
{
    public class LotExpirationService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<LotExpirationService> _logger;

        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

        public LotExpirationService(IServiceScopeFactory scopeFactory, ILogger<LotExpirationService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("LotExpirationService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanupExpiredLotsAsync();
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task CleanupExpiredLotsAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();

            var expiredLots = await db.Lots
                .Where(l => l.Status == "Active" && l.EndTime <= DateTime.UtcNow)
                .Include(l => l.Bids)
                .ToListAsync();

            foreach (var lot in expiredLots)
            {
                if (lot.Bids.Any())
                {
                    lot.Status = "Completed";
                    _logger.LogInformation("Lot {Id} completed with winner.", lot.Id);
                }
                else
                {
                    lot.Status = "Spoiled";
                    lot.ImageUrl = "/images/spoiled.png";
                    _logger.LogInformation("Lot {Id} marked as spoiled.", lot.Id);
                }
            }

            await db.SaveChangesAsync();
            _logger.LogInformation("Cleanup done. Processed {Count} expired lots.", expiredLots.Count);
        }
    }
}