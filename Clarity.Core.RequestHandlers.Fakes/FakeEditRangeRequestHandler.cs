namespace Clarity.Core.Fakes
{
    using Microsoft.EntityFrameworkCore;

    internal class FakeEditRangeRequestHandler : EditRangeRequestHandler<EditRangeRequest<FakeEntity>, FakeEntity>
    {
        internal FakeEditRangeRequestHandler(DbContext context) : base(context)
        {
        }
    }
}
