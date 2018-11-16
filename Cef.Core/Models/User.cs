namespace Cef.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;
    using Relationships;

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