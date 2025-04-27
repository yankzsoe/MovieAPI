using FluentValidation;
using System.Text.Json.Serialization;
using MovieAPI.Application;
using MovieAPI.Infrastructure;
using MovieAPI.WebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MovieAPI.WebAPI.Extensions;

namespace MovieAPI.WebAPI {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddInfrastructureServiceRegistration(builder.Configuration);
            builder.Services.AddApplicationServiceRegistration(builder.Configuration);

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });


            builder.Services.AddControllers().AddNewtonsoftJson()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwagger();

            var app = builder.Build();

            //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Continue;

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerExtension();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseMiddleware<ErrorHandler>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}