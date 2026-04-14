using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace VitacoreTestApp.Data
{
    public class AuctionDbContextFactory : IDesignTimeDbContextFactory<AuctionDbContext>
    {
        public AuctionDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var optionsBuilder = new DbContextOptionsBuilder<AuctionDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AuctionDbContext(optionsBuilder.Options);
        }
    }
}