using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieAPI.Application.Interfaces;
using MovieAPI.Domain.Entities;
using MovieAPI.Infrastructure.Persistance;

namespace MovieAPI.Infrastructure {
    public static class ServiceExtension {
        public static void AddInfrastructureServiceRegistration(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<AppDbContext>((serviceProvider, opt) => {
                //var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                opt.UseSqlServer(configuration.GetConnectionString("sqlserverdb"));
                opt.EnableSensitiveDataLogging(true);
                opt.EnableDetailedErrors();
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                // Konfigurasi password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false; // Contoh: !@#$
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Konfigurasi lockout (opsional)
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Konfigurasi user
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
