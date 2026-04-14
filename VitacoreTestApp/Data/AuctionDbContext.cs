using Microsoft.EntityFrameworkCore;
using VitacoreTestApp.ViewModels;

namespace VitacoreTestApp.Data { 
    public class AuctionDbContext : DbContext 
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связей
            modelBuilder.Entity<Bid>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bids)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Lot)
                .WithMany(l => l.Bids)
                .HasForeignKey(b => b.LotId)
                .OnDelete(DeleteBehavior.Cascade);

            // Индексы для оптимизации
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Lot>()
                .HasIndex(l => l.Status);
        }
    }
}
