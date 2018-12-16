namespace Cef.Core.Models
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Relationships;

    [PublicAPI]
    public class Order : BaseModel
    {
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
