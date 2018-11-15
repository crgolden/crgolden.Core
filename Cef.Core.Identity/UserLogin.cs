namespace Cef.Core.Identity
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;

    [PublicAPI]
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public virtual User User { get; set; }
    }
}
