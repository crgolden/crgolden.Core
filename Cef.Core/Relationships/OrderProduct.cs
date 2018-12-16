namespace Cef.Core.Relationships
{
    using JetBrains.Annotations;
    using Models;

    [PublicAPI]
    public class OrderProduct : BaseRelationship<Order, Product>
    {
        public decimal Quantity { get; set; }
    }
}
