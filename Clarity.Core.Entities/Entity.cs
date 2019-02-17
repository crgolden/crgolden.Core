namespace Clarity.Core
{
    using System;
    using Newtonsoft.Json;

    public abstract class Entity
    {
        [JsonProperty("created")]
        public DateTime Created { get; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; set; }

        protected Entity()
        {
            Created = DateTime.Now;
        }
    }
}
