namespace Cef.Core.Relationships
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;
    using Models;

    [PublicAPI]
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public virtual User User { get; set; }
    }
}
