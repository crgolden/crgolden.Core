namespace Cef.Core.Extensions
{
    using System.Threading.Tasks;
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    [PublicAPI]
    public static class WebHostExtensions
    {
        public static void SetupDatabase<TContext>(this IWebHost webHost) where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                using (var context = services.GetRequiredService<TContext>())
                {
                    context.Database.Migrate();
                    var seedDataService = services.GetRequiredService<ISeedDataService>();
                    Task.Run(seedDataService.SeedDatabase).Wait();
                }
            }
        }
    }
}