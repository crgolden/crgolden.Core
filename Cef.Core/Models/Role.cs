namespace Cef.Core.Models
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;
    using Relationships;

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