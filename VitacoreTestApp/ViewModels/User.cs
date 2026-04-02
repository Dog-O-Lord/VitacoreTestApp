using System.ComponentModel.DataAnnotations;

namespace VitacoreTestApp.ViewModels
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public List<Bid> Bids { get; set; } = new();
    }
}
