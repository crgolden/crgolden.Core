namespace Cef.Core.Relationships
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;
    using Models;

    [PublicAPI]
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}
