namespace Cef.Core.Identity
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;

    [PublicAPI]
    public class UserToken : IdentityUserToken<Guid>
    {
        public virtual User User { get; set; }
    }
}
