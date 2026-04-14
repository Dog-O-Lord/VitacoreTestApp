using Microsoft.EntityFrameworkCore;
using VitacoreTestApp.Data;
using VitacoreTestApp.ViewModels;

namespace VitacoreTestApp.Services
{
    public class AuctionService
    {
        private readonly AuctionDbContext _db;

        public AuctionService(AuctionDbContext db)
        {
            _db = db;
        }

        public async Task<List<Lot>> GetActiveLotsAsync()
        {
            return await _db.Lots
                .Where(l => l.Status == "Active")
                .OrderBy(l => l.EndTime)
                .ToListAsync();
        }

        public async Task<Lot?> GetLotWithBidsAsync(int id)
        {
            return await _db.Lots
                .Include(l => l.Bids)
                    .ThenInclude(b => b.User)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<(bool Success, string Message)> PlaceBidAsync(int lotId, int userId, decimal amount)
        {
            var lot = await _db.Lots
                .Include(l => l.Bids)
                .FirstOrDefaultAsync(l => l.Id == lotId);

            if (lot == null)
                return (false, "Lot not found.");

            if (lot.Status != "Active")
                return (false, "This auction is no longer active.");

            if (DateTime.UtcNow >= lot.EndTime)
                return (false, "This auction has ended.");

            if (amount <= lot.CurrentBid)
                return (false, $"Your bid must be higher than the current bid of {lot.CurrentBid:C}.");

            if (amount >= lot.BuyoutPrice)
                return (false, $"Your bid exceeds the buyout price. Use the buyout option instead.");

            var previousTopBid = lot.Bids
                .OrderByDescending(b => b.Amount)
                .FirstOrDefault();

            var bid = new Bid
            {
                LotId = lotId,
                UserId = userId,
                Amount = amount,
                Timestamp = DateTime.UtcNow
            };

            lot.CurrentBid = amount;
            _db.Bids.Add(bid);
            await _db.SaveChangesAsync();

            return (true, previousTopBid?.UserId.ToString() ?? "");
        }

        public async Task<(bool Success, string Message)> BuyoutAsync(int lotId, int userId)
        {
            var lot = await _db.Lots.FirstOrDefaultAsync(l => l.Id == lotId);

            if (lot == null)
                return (false, "Lot not found.");

            if (lot.Status != "Active")
                return (false, "This auction is no longer active.");

            if (DateTime.UtcNow >= lot.EndTime)
                return (false, "This auction has ended.");

            lot.Status = "Completed";
            lot.CurrentBid = lot.BuyoutPrice;

            var bid = new Bid
            {
                LotId = lotId,
                UserId = userId,
                Amount = lot.BuyoutPrice,
                Timestamp = DateTime.UtcNow
            };

            _db.Bids.Add(bid);
            await _db.SaveChangesAsync();

            return (true, "Congratulations! You bought out this lot.");
        }

        public async Task<User?> GetUserAsync(int userId)
        {
            return await _db.Users.FindAsync(userId);
        }
    }
}