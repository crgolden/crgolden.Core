namespace Cef.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using Newtonsoft.Json;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class BaseModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; set; }
    }
}
