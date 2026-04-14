using VitacoreTestApp.Data;
using VitacoreTestApp.ViewModels;

namespace VitacoreTestApp.Services
{
    public class LotGeneratorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<LotGeneratorService> _logger;

        private readonly TimeSpan _interval = TimeSpan.FromMinutes(2);

        public LotGeneratorService(IServiceScopeFactory scopeFactory, ILogger<LotGeneratorService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("LotGeneratorService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await GenerateLotAsync();
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task GenerateLotAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();

            var lot = new Lot
            {
                Description = $"Fresh Mandarine Lot #{Random.Shared.Next(1000, 9999)}",
                ImageUrl = "/images/fresh.png",
                Status = "Active",
                BuyoutPrice = Random.Shared.Next(50, 500),
                CurrentBid = Random.Shared.Next(5, 49),
                CreatedAt = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(24)
            };

            db.Lots.Add(lot);
            await db.SaveChangesAsync();
            _logger.LogInformation("Generated new lot: {Description}", lot.Description);
        }
    }
}