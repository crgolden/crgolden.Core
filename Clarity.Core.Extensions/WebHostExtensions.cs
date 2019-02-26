namespace Clarity.Core
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class WebHostExtensions
    {
        public static async Task<IWebHost> MigrateDatabaseAsync(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DbContext>();
                await context.Database.MigrateAsync();
            }

            return webHost;
        }

        public static IWebHost GetQueueClients(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                scope.ServiceProvider.GetServices<IQueueClient>();
            }

            return webHost;
        }
    }
}
