namespace Clarity.Core
{
    using System;
    using Newtonsoft.Json;

    public abstract class Entity
    {
        [JsonProperty("created")]
        public DateTime Created { get; private set; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; set; }

        protected Entity()
        {
            Created = DateTime.Now;
        }
    }
}
