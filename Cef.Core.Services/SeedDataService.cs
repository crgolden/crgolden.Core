namespace Cef.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Identity;
    using Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using UserOptions = Options.UserOptions;

    public class SeedDataService : ISeedDataService
    {
        private readonly DbContext _context;
        private readonly UserOptions _userOptions;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public SeedDataService(DbContext context, IOptions<UserOptions> userOptions,
            UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userOptions = userOptions.Value;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDatabase()
        {
            if (!await _roleManager.Roles.AnyAsync()) await CreateRolesAsync();
            if (!await _userManager.Users.AnyAsync()) await CreateUsersAsync();
        }

        private async Task CreateRolesAsync()
        {
            using (_roleManager)
            {
                foreach (var role in SeedData.Roles)
                {
                    await _roleManager.CreateAsync(role);
                }
            }
        }

        private async Task CreateUsersAsync()
        {
            using (_userManager)
            {
                foreach (var userOption in _userOptions.Users)
                {
                    var user = new User
                    {
                        UserName = userOption.Email,
                        Email = userOption.Email,
                        FirstName = userOption.FirstName,
                        LastName = userOption.LastName,
                        SecurityStamp = $"{Guid.NewGuid()}"
                    };
                    await _userManager.CreateAsync(user, userOption.Password);
                    await _userManager.AddToRolesAsync(user, SeedData.Roles.Select(role => role.Name));
                    await _userManager.AddClaimsAsync(user, SeedData.Claims);
                }
            }
        }
    }

    public static class SeedData
    {
        public static IEnumerable<Claim> Claims => new List<Claim>
        {
        };

        public static IEnumerable<Role> Roles => new List<Role>
        {
            new Role("User"),
            new Role("Admin")
        };
    }
}