namespace Clarity.Core
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    [ExcludeFromCodeCoverage]
    public static class WebHostExtensions
    {
        public static async Task<IWebHost> MigrateDatabaseAsync(this IWebHost webHost, CancellationToken token)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DbContext>();
                await context.Database.MigrateAsync(token);
            }

            return webHost;
        }
    }
}
