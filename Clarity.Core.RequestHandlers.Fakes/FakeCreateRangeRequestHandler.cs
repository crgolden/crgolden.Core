namespace Clarity.Core.Fakes
{
    using System.Collections.Generic;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeCreateRangeRequestHandler
        : CreateRangeRequestHandler<CreateRangeRequest<IEnumerable<Model>, FakeEntity, Model>, IEnumerable<Model>, FakeEntity, Model>
    {
        internal FakeCreateRangeRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
