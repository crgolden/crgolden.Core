namespace Clarity.Core.Fakes
{
    using Microsoft.EntityFrameworkCore;

    internal class FakeIndexRequestHandler : IndexRequestHandler<IndexRequest, FakeEntity>
    {
        internal FakeIndexRequestHandler(DbContext context) : base(context)
        {
        }
    }
}
