using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MovieAPI.Application.Common.Behaviours;
using MovieAPI.Application.Interfaces;
using MovieAPI.Application.Services;

namespace MovieAPI.Application {
    public static class ServiceExtension {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration) {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(cfg =>
                    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                );

            services.AddTransient<IDateTime, DateTimeService>();
        }
    }
}
