namespace Cef.Core.Identity
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;

    [PublicAPI]
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
