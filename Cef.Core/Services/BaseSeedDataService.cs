namespace Cef.Core.Services
{
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public abstract class BaseSeedDataService<TContext> : ISeedDataService<TContext>
        where TContext : DbContext
    {
        public TContext Context { get; set; }

        protected BaseSeedDataService(TContext context)
        {
            Context = context;
        }

        public abstract Task SeedDatabase();
    }
}