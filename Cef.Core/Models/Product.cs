namespace Cef.Core.Models
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Relationships;

    [PublicAPI]
    public class Product : BaseModel
    {
        public string Description { get; set; }

        public decimal Price { get; set; }

        public string PictureFileName { get; set; }

        public string PictureUri { get; set; }

        public virtual ICollection<CartProduct> CartProducts { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
