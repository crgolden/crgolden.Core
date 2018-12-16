namespace Cef.Core.Extensions
{
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Relationships;

    [PublicAPI]
    public static class ModelBuilderExtensions
    {
        public static void ConfigureIdentityContext(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
                b.ToTable("Users");
            });

            modelBuilder.Entity<UserClaim>(b =>
            {
                b.ToTable("UserClaims");
            });

            modelBuilder.Entity<UserLogin>(b =>
            {
                b.ToTable("UserLogins");
            });

            modelBuilder.Entity<UserToken>(b =>
            {
                b.ToTable("UserTokens");
            });

            modelBuilder.Entity<Role>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
                b.ToTable("Roles");
            });

            modelBuilder.Entity<RoleClaim>(b =>
            {
                b.ToTable("RoleClaims");
            });

            modelBuilder.Entity<UserRole>(b =>
            {
                b.ToTable("UserRoles");
            });
        }

        public static void ConfigureECommerceContext(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.HasOne(e => e.Cart).WithOne(e => e.User).HasForeignKey<Cart>(e => e.UserId);
                b.HasMany(e => e.Orders).WithOne(e => e.User).HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<Cart>(b =>
            {
                b.HasMany(e => e.CartProducts).WithOne(e => e.Model1).HasForeignKey(e => e.Model1Id);
                b.ToTable("Carts");
            });

            modelBuilder.Entity<Order>(b =>
            {
                b.HasOne(e => e.User).WithMany(e => e.Orders).HasForeignKey(e => e.UserId);
                b.HasMany(e => e.OrderProducts).WithOne(e => e.Model1).HasForeignKey(e => e.Model1Id);
                b.ToTable("Orders");
            });

            modelBuilder.Entity<Product>(b =>
            {
                b.Property(e => e.Price).HasColumnType("decimal(18,2)");
                b.HasMany(e => e.CartProducts).WithOne(e => e.Model2).HasForeignKey(e => e.Model2Id);
                b.HasMany(e => e.OrderProducts).WithOne(e => e.Model2).HasForeignKey(e => e.Model2Id);
                b.ToTable("Products");
            });

            modelBuilder.Entity<CartProduct>(b =>
            {
                b.HasKey(e => new { e.Model1Id, e.Model2Id });
                b.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
                b.HasOne(e => e.Model1).WithMany(e => e.CartProducts).HasForeignKey(e => e.Model1Id);
                b.HasOne(e => e.Model2).WithMany(e => e.CartProducts).HasForeignKey(e => e.Model2Id);
                b.ToTable("CartProducts");
            });

            modelBuilder.Entity<OrderProduct>(b =>
            {
                b.HasKey(e => new { e.Model1Id, e.Model2Id });
                b.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
                b.HasOne(e => e.Model1).WithMany(e => e.OrderProducts).HasForeignKey(e => e.Model1Id);
                b.HasOne(e => e.Model2).WithMany(e => e.OrderProducts).HasForeignKey(e => e.Model2Id);
                b.ToTable("OrderProducts");
            });
        }
    }
}