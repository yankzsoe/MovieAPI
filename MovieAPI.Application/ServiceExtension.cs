using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MovieAPI.Application.Common.Behaviours;
using MovieAPI.Application.Interfaces;
using MovieAPI.Application.Services;
using FluentValidation.AspNetCore;
using MovieAPI.Application.Features.Movie.Commands.Create;
using Microsoft.AspNetCore.Mvc;

namespace MovieAPI.Application {
    public static class ServiceExtension {
        public static void AddApplicationServiceRegistration(this IServiceCollection services, IConfiguration configuration) {
            // Add FluentValidation
            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(MovieCreateValidator).Assembly);

            // Add Middleware
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            
            


            services.Configure<ApiBehaviorOptions>(options => {
                options.SuppressModelStateInvalidFilter = true;
            });


            // Add AutoMapper and MediatR
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(cfg =>
                    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                );

            services.AddSingleton<IDateTime, DateTimeService>();
        }
    }
}
