namespace Clarity.Core
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    [PublicAPI]
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
    }
}