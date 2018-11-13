namespace Cef.Core.Relationships
{
    using System;
    using Models;
    using Microsoft.AspNetCore.Identity;

    public class UserLogin : IdentityUserLogin<Guid>
    {
        public virtual User User { get; set; }
    }
}
