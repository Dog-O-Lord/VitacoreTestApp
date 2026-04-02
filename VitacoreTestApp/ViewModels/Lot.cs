using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VitacoreTestApp.ViewModels
{
        public class Lot    
        {
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Флаг для легкой очистки "испорченных" лотов
        public bool IsExpired { get; set; } = false;

        // Список всех ставок на этот конкретный лот
        public List<Bid> Bids { get; set; } = new();
    } 

}