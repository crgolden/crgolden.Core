namespace Clarity.Core
{
    using System;

    public abstract class Entity
    {
        public DateTime Created { get; }

        public DateTime? Updated { get; set; }
    }
}
