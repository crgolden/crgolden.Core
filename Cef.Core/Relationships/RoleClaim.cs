namespace Cef.Core.Relationships
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;
    using Models;

    [PublicAPI]
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual Role Role { get; set; }
    }
}
