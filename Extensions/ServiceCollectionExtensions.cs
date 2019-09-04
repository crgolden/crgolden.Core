namespace crgolden.Core
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityServerAuthentication(
            this IServiceCollection services,
            IConfiguration configuration,
            string apiName,
            string defaultScheme = "Bearer")
        {
            services.AddAuthentication(defaultScheme)
                .AddIdentityServerAuthentication(defaultScheme, options =>
                {
                    var identityServerAddress = configuration.GetValue<string>("IdentityServerAddress");
                    if (string.IsNullOrEmpty(identityServerAddress)) return;

                    options.Authority = identityServerAddress;
                    options.ApiName = apiName;
                    options.RoleClaimType = ClaimTypes.Role;
                });
            return services;
        }
    }
}
