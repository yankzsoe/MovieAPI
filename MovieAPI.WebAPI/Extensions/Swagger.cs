using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MovieAPI.WebAPI.Extensions {
    public static class Swagger {
        public static void AddSwagger(this IServiceCollection services) {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Movie API",
                    Version = "v1",
                    Description = "REST API for Movie API application"
                }); ;
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    Description = "Input your token in this format - Bearer {access token} to access this API",
                });
                c.OperationFilter<AuthResponsesOperationFilter>();
                c.TagActionsBy(api => {
                    if (api.GroupName != null) {
                        return new[] { api.GroupName };
                    }

                    var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                    if (controllerActionDescriptor != null) {
                        return new[] { controllerActionDescriptor.ControllerName };
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });
                c.DocInclusionPredicate((name, api) => true);
                c.DescribeAllParametersInCamelCase();
            });
        }

        public class AuthResponsesOperationFilter : IOperationFilter {
            public void Apply(OpenApiOperation operation, OperationFilterContext context) {
                var actionMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
                var isAuthorized = actionMetadata.Any(metadataItem => metadataItem is AuthorizeAttribute);
                var allowAnonymous = actionMetadata.Any(metadataItem => metadataItem is AllowAnonymousAttribute);

                if (!isAuthorized || allowAnonymous) return;
                if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme,
                                },
                                Scheme = JwtBearerDefaults.AuthenticationScheme,
                                Name = JwtBearerDefaults.AuthenticationScheme,
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        },
                    }
                };
            }
        }

        public static void UseSwaggerExtension(this IApplicationBuilder app) {
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieAPI.WebAPI v1");
                c.DefaultModelsExpandDepth(-1);
            });
        }
    }
}
