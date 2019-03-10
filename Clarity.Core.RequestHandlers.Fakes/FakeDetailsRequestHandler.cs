namespace Clarity.Core.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeDetailsRequestHandler : DetailsRequestHandler<DetailsRequest<FakeEntity, Model>, FakeEntity, Model>
    {
        internal FakeDetailsRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
