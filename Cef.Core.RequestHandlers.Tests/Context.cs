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
            modelBuilder.Entity<Model>(entity =>
            {
                entity.Property(model => model.Id);
                entity.Property(model => model.Name);
                entity.Property(model => model.TestName);
            });
            modelBuilder.Entity<Relationship>(entity =>
            {
                entity.HasKey(relationship => new
                {
                    relationship.Model1Id,
                    relationship.Model2Id
                });
                entity.Property(relationship => relationship.Model1Name);
                entity.Property(relationship => relationship.Model2Name);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}