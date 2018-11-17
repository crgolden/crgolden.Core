﻿namespace Cef.Core.Extensions
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
        public static async Task<IWebHost> SeedDatabaseAsync(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<IWebHost>>();
                var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
                try
                {
                    logger.LogInformation("Seeding tha database");
                    await seedService.SeedAsync();
                    logger.LogInformation("Seeded the database");
                }
                catch (Exception e)
                {
                    logger.LogError(e, "An error occurred while seeding the database");
                }
            }

            return webHost;
        }
    }
}