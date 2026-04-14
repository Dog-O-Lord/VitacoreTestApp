using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using VitacoreTestApp.Data;
using VitacoreTestApp.Services;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Design;

namespace VitacoreTestApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Добавление DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            builder.Services.AddDbContext<AuctionDbContext>(options =>
                options.UseSqlServer(connectionString));
            
            // Добавление сервисов аутентификации
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            // Регистрация пользовательских сервисов
            builder.Services.AddScoped<AuctionServise>();
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<LotExpirationService>();

            // Добавление контроллеров и представлений
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Middleware конфигурация
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Lots}/{action=Index}/{id?}");

            // Запуск сервиса истечения лотов (если он имеет фоновую работу)
            var lotExpirationService = app.Services.GetRequiredService<LotExpirationService>();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
                if (dbContext.Database.CanConnect())
                {
                    Console.WriteLine("✅ Database connected successfully!");
                }
                else
                {
                    Console.WriteLine("❌ Failed to connect to database");
                }
            }

            app.Run();
        }
    }
}
