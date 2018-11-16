namespace Cef.Core.Interfaces
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;

    [PublicAPI]
    public interface ISeedDataService<TContext> where TContext : DbContext
    {
        TContext Context { get; set; }

        Task SeedDatabase();
    }
}