using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;

namespace VitacoreTestApp.ViewModels
{
        public class Lot
        {
                public int Id { get; set; }
                public string ImageUrl { get; set; }
                public string Description { get; set; }
                public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
                public string Status { get; set; } = "Active"; // Active / Completed / Spoiled
                public decimal BuyoutPrice { get; set; }
                public decimal CurrentBid { get; set; }
                public DateTime EndTime { get; set; }
    
                public ICollection<Bid> Bids { get; set; }
        }
        } 

