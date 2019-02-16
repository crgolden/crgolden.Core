namespace Clarity.Core.Fakes
{
    using Microsoft.EntityFrameworkCore;

    internal class FakeCreateRequestHandler : CreateRequestHandler<CreateRequest<FakeEntity>, FakeEntity>
    {
        internal FakeCreateRequestHandler(DbContext context) : base(context)
        {
        }
    }
}
