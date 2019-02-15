namespace Cef.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using Newtonsoft.Json;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class RelationshipBase<T1, T2>
        where T1 : ModelBase
        where T2 : ModelBase
    {
        [Required]
        [JsonProperty("model1Id")]
        public Guid Model1Id { get; }

        [Required]
        [JsonProperty("model1Name")]
        public string Model1Name { get; }

        [JsonIgnore]
        public T1 Model1 { get; }

        [Required]
        [JsonProperty("model2Id")]
        public Guid Model2Id { get; }

        [Required]
        [JsonProperty("model2Name")]
        public string Model2Name { get; }

        [JsonIgnore]
        public T2 Model2 { get; }

        [JsonProperty("created")]
        public readonly DateTime Created;

        [JsonProperty("updated")]
        public DateTime? Updated { get; set; }

        protected RelationshipBase(Guid model1Id, string model1Name, Guid model2Id, string model2Name)
        {
            Model1Id = model1Id;
            Model1Name = model1Name;
            Model2Id = model2Id;
            Model2Name = model2Name;
            Created = DateTime.UtcNow;
        }
    }
}
