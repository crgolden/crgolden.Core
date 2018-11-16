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
        public static async Task<IWebHost> MigrateDatabaseAsync<TContext>(this IWebHost webHost)
            where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var seedDataService = services.GetService<ISeedDataService<TContext>>();
                using (var context = services.GetRequiredService<TContext>())
                {
                    try
                    {
                        logger.LogInformation($"Migrating database associated with context {nameof(TContext)}");
                        await context.Database.MigrateAsync();
                        if (seedDataService != null)
                        {
                            await seedDataService.SeedDatabase();
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"An error occurred while migrating the database used on context {nameof(TContext)}");
                    }
                }
            }

            return webHost;
        }
    }
}