namespace Cef.Core.Extensions
{
    using Options;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static void AddCorsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var corsOptionsSection = configuration.GetSection(nameof(CorsOptions));
            if (!corsOptionsSection.Exists())
            {
                return;
            }

            services.Configure<CorsOptions>(options =>
            {
                var corsOptions = corsOptionsSection.Get<CorsOptions>();
                options.Origins = corsOptions.Origins;
            });
        }

        public static void AddUserOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var usersOptionsSection = configuration.GetSection(nameof(UserOptions));
            if (!usersOptionsSection.Exists())
            {
                return;
            }

            services.Configure<UserOptions>(options =>
            {
                var usersOptions = usersOptionsSection.Get<UserOptions>();
                options.Users = usersOptions.Users;
            });
        }

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationOptionsSection = configuration.GetSection(nameof(AuthenticationOptions));
            if (!authenticationOptionsSection.Exists())
            {
                return;
            }

            services.Configure<AuthenticationOptions>(options =>
            {
                var authenticationOptions = authenticationOptionsSection.Get<AuthenticationOptions>();
                options.Facebook = authenticationOptions.Facebook;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public static void AddEmailOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var emailOptionsSection = configuration.GetSection(nameof(EmailOptions));
            if (!emailOptionsSection.Exists())
            {
                return;
            }

            services.Configure<EmailOptions>(options =>
            {
                var emailOptions = emailOptionsSection.Get<EmailOptions>();
                options.ApiKey = emailOptions.ApiKey;
                options.Email = emailOptions.Email;
                options.Name = emailOptions.Name;
            });
        }

        public static void AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
            });
        }
    }
}
