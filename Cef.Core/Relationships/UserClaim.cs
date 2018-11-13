namespace Cef.Core.Relationships
{
    using System;
    using Models;
    using Microsoft.AspNetCore.Identity;

    public class UserClaim : IdentityUserClaim<Guid>
    {
        public virtual User User { get; set; }
    }
}
