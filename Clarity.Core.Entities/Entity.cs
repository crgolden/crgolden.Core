namespace Clarity.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public abstract class Entity
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("created")]
        public DateTime Created { get; private set; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; set; }

        protected Entity(string name)
        {
            Name = name;
            Created = DateTime.Now;
        }
    }
}
