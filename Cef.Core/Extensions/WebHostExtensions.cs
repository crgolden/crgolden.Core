namespace Cef.Core.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    [PublicAPI]
    public static class WebHostExtensions
    {
        public static async Task<IWebHost> SetupDatabaseAsync<TContext>(this IWebHost webHost) where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
                var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
                using (var context = scope.ServiceProvider.GetRequiredService<TContext>())
                {
                    var message = $"the database associated with context {typeof(TContext).Name}";
                    try
                    {
                        logger.LogInformation($"Migrating {message}");
                        await context.Database.MigrateAsync();
                        await seedService.SeedAsync();
                        logger.LogInformation($"Migrated {message}");
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"An error occurred while migrating {message}");
                    }
                }
            }

            return webHost;
        }
    }
}