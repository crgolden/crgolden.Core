namespace Cef.Core.Interfaces
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;

    [PublicAPI]
    public interface ISeedDataService<out TContext> where TContext : DbContext
    {
        TContext Context { get; }

        Task SeedDatabase();
    }
}