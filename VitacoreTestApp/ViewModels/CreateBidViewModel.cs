using System.ComponentModel.DataAnnotations;

namespace VitacoreTestApp.ViewModels
{
    public class CreateBidViewModel
    {
        [Required(ErrorMessage = "Лот обязателен")]
        public int LotId { get; set; }

        [Required(ErrorMessage = "Сумма ставки обязательна")]
        [Range(0.01, 999999999.99, ErrorMessage = "Сумма должна быть больше 0")]
        [Display(Name = "Сумма ставки")]
        public decimal Amount { get; set; }

        [Display(Name = "Текущая максимальная ставка")]
        public decimal CurrentHighestBid { get; set; }

        [Display(Name = "Минимальный шаг ставки")]
        public decimal MinimumBidIncrement { get; set; } = 10m;

        public string LotName { get; set; } = string.Empty;

        [Display(Name = "Время окончания")]
        public DateTime LotEndTime { get; set; }

        public bool IsAuctionEnded => DateTime.UtcNow >= LotEndTime;

    }
}