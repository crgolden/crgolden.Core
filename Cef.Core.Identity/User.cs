namespace Cef.Core.Identity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;

    [PublicAPI]
    public class User : IdentityUser<Guid>
    {
        public string Name => $"{FirstName} {LastName}";
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}