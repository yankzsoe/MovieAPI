﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieAPI.Application.Interfaces;
using MovieAPI.Infrastructure.Persistance;

namespace MovieAPI.Infrastructure {
    public static class ServiceExtension {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<AppDbContext>(opt => {
                opt.UseSqlServer(configuration.GetConnectionString("sqlserverdb"));
                opt.EnableSensitiveDataLogging(true);
                opt.EnableDetailedErrors();
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
