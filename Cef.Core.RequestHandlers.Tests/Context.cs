namespace Cef.Core.RequestHandlers.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;

    [ExcludeFromCodeCoverage]
    internal class Context : DbContext
    {
        internal Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Model>();
            modelBuilder.Entity<Relationship>().HasKey(p => new { p.Model1Id, p.Model2Id });
        }
    }
}