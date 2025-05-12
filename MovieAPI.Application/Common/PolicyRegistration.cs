using Microsoft.Extensions.DependencyInjection;
using MovieAPI.Application.Common.Models;

namespace MovieAPI.Application.Common {
    public static class PolicyRegistration {
        public static void AddAuthorizationPolicies(this IServiceCollection services) {
            services.AddAuthorization(opt => {
                opt.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                opt.AddPolicy(nameof(PolicyModel.GetMovie), policy => {
                    policy.RequireClaim(PolicyModel.GetMovie.Type, PolicyModel.GetMovie.Name);
                });

                opt.AddPolicy(nameof(PolicyModel.CreateMovie), policy => {
                    policy.RequireClaim(PolicyModel.CreateMovie.Type, PolicyModel.CreateMovie.Name);
                });

                opt.AddPolicy(nameof(PolicyModel.UpdateMovie), policy => {
                    policy.RequireClaim(PolicyModel.UpdateMovie.Type, PolicyModel.UpdateMovie.Name);
                });

                opt.AddPolicy(nameof(PolicyModel.DeleteMovie), policy => {
                    policy.RequireClaim(PolicyModel.DeleteMovie.Type, PolicyModel.DeleteMovie.Name);
                });
            });
        }
    }
}
