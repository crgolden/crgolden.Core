namespace Clarity.Core.Fakes
{
    using Microsoft.EntityFrameworkCore;

    internal class FakeDeleteRequestHandler : DeleteRequestHandler<DeleteRequest, FakeEntity>
    {
        internal FakeDeleteRequestHandler(DbContext context) : base(context)
        {
        }
    }
}
