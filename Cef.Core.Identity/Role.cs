namespace Cef.Core.Identity
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;

    [PublicAPI]
    public class Role : IdentityRole<Guid>
    {
        public Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    }
}