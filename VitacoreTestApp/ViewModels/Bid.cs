using System.ComponentModel.DataAnnotations;

namespace VitacoreTestApp.ViewModels
{
    public class Bid
    {
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int LotId { get; set; }
        public Lot Lot { get; set; } = null!;
    }
}