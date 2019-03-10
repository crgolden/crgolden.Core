namespace Clarity.Core
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public abstract class FileConfiguration<TEntity> : EntityConfiguration<TEntity>
        where TEntity : File
    {
        public override void Configure(EntityTypeBuilder<TEntity> file)
        {
            base.Configure(file);
            file.Property(e => e.Uri).IsRequired();
            file.HasIndex(e => e.Uri).IsUnique();
            file.Property(e => e.Name).IsRequired();
            file.HasIndex(e => e.Name);
            file.Property(e => e.ContentType);
            file.Property(e => e.Size).IsRequired();
            file.ToTable("Files");
        }
    }
}
