namespace Cef.Core.Extensions
{
    using System.Threading.Tasks;
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    [PublicAPI]
    public static class WebHostExtensions
    {
        public static async Task<IWebHost> SeedDatabaseAsync(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seedDataService = services.GetRequiredService<ISeedDataService>();
                await seedDataService.SeedDatabase();
            }

            return webHost;
        }
    }
}