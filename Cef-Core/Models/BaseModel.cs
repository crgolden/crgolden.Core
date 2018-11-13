namespace Cef_Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class BaseModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
