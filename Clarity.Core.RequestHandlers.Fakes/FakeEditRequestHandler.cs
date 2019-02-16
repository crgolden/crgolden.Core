namespace Clarity.Core.Fakes
{
    using Microsoft.EntityFrameworkCore;

    internal class FakeEditRequestHandler : EditRequestHandler<EditRequest<FakeEntity>, FakeEntity>
    {
        internal FakeEditRequestHandler(DbContext context) : base(context)
        {
        }
    }
}
