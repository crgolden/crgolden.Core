namespace Cef.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using Newtonsoft.Json;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class ModelBase
    {
        [JsonProperty("id")]
        public Guid Id { get; private set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("created")]
        public readonly DateTime Created;

        [JsonProperty("updated")]
        public DateTime? Updated { get; set; }

        protected ModelBase(string name)
        {
            Name = name;
            Created = DateTime.Now;
        }

        protected ModelBase(string name, Guid id) : this(name)
        {
            Id = id;
        }
    }
}
