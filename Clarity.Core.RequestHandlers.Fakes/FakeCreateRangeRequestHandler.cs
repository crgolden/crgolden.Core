namespace Clarity.Core.Fakes
{
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    internal class FakeCreateRangeRequestHandler
        : CreateRangeRequestHandler<CreateRangeRequest<IEnumerable<FakeEntity>, FakeEntity>, IEnumerable<FakeEntity>, FakeEntity>
    {
        internal FakeCreateRangeRequestHandler(DbContext context) : base(context)
        {
        }
    }
}
