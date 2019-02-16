namespace Clarity.Core.Fakes
{
    using Microsoft.EntityFrameworkCore;

    internal class FakeDetailsRequestHandler : DetailsRequestHandler<DetailsRequest<FakeEntity>, FakeEntity>
    {
        internal FakeDetailsRequestHandler(DbContext context) : base(context)
        {
        }
    }
}
