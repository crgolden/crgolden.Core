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
        [JsonProperty("model1Id")]
        public Guid Model1Id { get; set; }

        [Required]
        [JsonProperty("model1Name")]
        public string Model1Name { get; set; }

        [JsonIgnore]
        public T1 Model1 { get; set; }

        [Required]
        [JsonProperty("model2Id")]
        public Guid Model2Id { get; set; }

        [Required]
        [JsonProperty("model2Name")]
        public string Model2Name { get; set; }

        [JsonIgnore]
        public T2 Model2 { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; set; }
    }
}
