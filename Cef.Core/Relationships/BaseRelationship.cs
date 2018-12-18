namespace Cef.Core.Relationships
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Models;
    using JetBrains.Annotations;
    using Newtonsoft.Json;

    [PublicAPI]
    public abstract class BaseRelationship<T1, T2> where T1 : BaseModel where T2 : BaseModel
    {
        [Required]
        public Guid Model1Id { get; set; }

        [Required]
        public string Model1Name { get; set; }

        [JsonIgnore]
        public T1 Model1 { get; set; }

        [Required]
        public Guid Model2Id { get; set; }

        [Required]
        public string Model2Name { get; set; }

        [JsonIgnore]
        public T2 Model2 { get; set; }
    }
}
