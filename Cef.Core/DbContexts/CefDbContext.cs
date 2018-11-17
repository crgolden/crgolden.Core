namespace Cef.Core.DbContexts
{
    using System;
    using IdentityServer4.EntityFramework.Entities;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Relationships;
    using UserClaim = Relationships.UserClaim;

    [PublicAPI]
    public class CefDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public CefDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            // ConfigurationDbContext
            modelBuilder.Entity<Client>(b =>
            {
                b.ToTable("Clients");
            });

            modelBuilder.Entity<ClientSecret>(b =>
            {
                b.ToTable("ClientSecrets");
            });

            modelBuilder.Entity<ClientGrantType>(b =>
            {
                b.ToTable("ClientGrantTypes");
            });

            modelBuilder.Entity<ClientRedirectUri>(b =>
            {
                b.ToTable("ClientRedirectUris");
            });

            modelBuilder.Entity<ClientPostLogoutRedirectUri>(b =>
            {
                b.ToTable("ClientPostLogoutRedirectUris");
            });
            
            modelBuilder.Entity<ClientScope>(b =>
            {
                b.ToTable("ClientScopes");
            });
            
            modelBuilder.Entity<ClientIdPRestriction>(b =>
            {
                b.ToTable("ClientIdPRestrictions");
            });
            
            modelBuilder.Entity<ClientClaim>(b =>
            {
                b.ToTable("ClientClaims");
            });
            
            modelBuilder.Entity<ClientCorsOrigin>(b =>
            {
                b.ToTable("ClientCorsOrigins");
            });
            
            modelBuilder.Entity<ClientProperty>(b =>
            {
                b.ToTable("ClientProperties");
            });
            
            modelBuilder.Entity<IdentityResource>(b =>
            {
                b.ToTable("IdentityResources");
            });

            modelBuilder.Entity<IdentityClaim>(b =>
            {
                b.ToTable("IdentityClaims");
            });

            modelBuilder.Entity<ApiResource>(b =>
            {
                b.ToTable("ApiResources");
            });

            modelBuilder.Entity<ApiSecret>(b =>
            {
                b.ToTable("ApiSecrets");
            });

            modelBuilder.Entity<ApiScope>(b =>
            {
                b.ToTable("ApiScopes");
            });

            modelBuilder.Entity<ApiResourceClaim>(b =>
            {
                b.ToTable("ApiResourceClaims");
            });

            // PersistedGrantDbContext
            modelBuilder.Entity<PersistedGrant>(b =>
            {
                b.ToTable("PersistedGrants");
            });
        }
    }
}