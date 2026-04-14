using System;

namespace VitacoreTestApp.ViewModels
{
    public class LotDetailsViewModel
    {
        public Lot Lot { get; set; } = null!;
        public User? CurrentUser { get; set; }
        public List<Bid> Bids { get; set; } = new();
        public decimal? UserHighestBid { get; set; }
        public bool IsAuctionEnded => DateTime.UtcNow > Lot.EndTime;
        public TimeSpan TimeRemaining => Lot.EndTime > DateTime.UtcNow
            ? Lot.EndTime - DateTime.UtcNow
            : TimeSpan.Zero;
    }
}