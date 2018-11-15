namespace Cef.Core.Identity
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;

    [PublicAPI]
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public virtual User User { get; set; }
    }
}
