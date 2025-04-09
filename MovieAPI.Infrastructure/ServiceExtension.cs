using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieAPI.Application.Interfaces;
using MovieAPI.Infrastructure.Persistance;

namespace MovieAPI.Infrastructure {
    public static class ServiceExtension {
        public static void InfrastructureServiceRegistration(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<AppDbContext>((serviceProvider, opt) => {
                //var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                opt.UseSqlServer(configuration.GetConnectionString("sqlserverdb"));
                opt.EnableSensitiveDataLogging(true);
                opt.EnableDetailedErrors();
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
