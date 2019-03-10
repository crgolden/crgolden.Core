namespace Clarity.Core.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeEditRangeRequestHandler : EditRangeRequestHandler<EditRangeRequest<FakeEntity, Model>, FakeEntity, Model>
    {
        internal FakeEditRangeRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
