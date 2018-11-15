namespace Cef.Core.Identity
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;

    [PublicAPI]
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual Role Role { get; set; }
    }
}
