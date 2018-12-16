namespace Cef.Core.Relationships
{
    using JetBrains.Annotations;
    using Models;

    [PublicAPI]
    public class CartProduct : BaseRelationship<Cart, Product>
    {
        public decimal Quantity { get; set; }
    }
}
