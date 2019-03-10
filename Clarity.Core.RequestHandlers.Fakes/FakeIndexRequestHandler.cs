namespace Clarity.Core.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeIndexRequestHandler : IndexRequestHandler<IndexRequest<FakeEntity, Model>, FakeEntity, Model>
    {
        internal FakeIndexRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
