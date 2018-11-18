namespace Cef.Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using JetBrains.Annotations;

    [PublicAPI]
    public abstract class BaseModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
