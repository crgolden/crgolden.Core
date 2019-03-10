namespace Clarity.Core.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeCreateRequestHandler : CreateRequestHandler<CreateRequest<FakeEntity, Model>, FakeEntity, Model>
    {
        internal FakeCreateRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
