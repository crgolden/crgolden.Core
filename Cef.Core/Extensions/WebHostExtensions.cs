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
                var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
                await seedService.SeedAsync();
            }

            return webHost;
        }
    }
}