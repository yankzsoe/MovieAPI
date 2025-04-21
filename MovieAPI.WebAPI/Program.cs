using FluentValidation;
using System.Text.Json.Serialization;
using MovieAPI.Application;
using MovieAPI.Infrastructure;
using MovieAPI.WebAPI.Middlewares;

namespace MovieAPI.WebAPI {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationServiceRegistration(builder.Configuration);
            builder.Services.AddInfrastructureServiceRegistration(builder.Configuration);

            builder.Services.AddControllers().AddNewtonsoftJson()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseMiddleware<ErrorHandler>();

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}