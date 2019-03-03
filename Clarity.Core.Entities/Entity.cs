namespace Clarity.Core
{
    using System;

    public abstract class Entity
    {
        public DateTime Created { get; private set; }

        public DateTime? Updated { get; set; }
    }
}
