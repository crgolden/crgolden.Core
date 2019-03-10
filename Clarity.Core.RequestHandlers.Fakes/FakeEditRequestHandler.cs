namespace Clarity.Core.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeEditRequestHandler : EditRequestHandler<EditRequest<FakeEntity, Model>, FakeEntity, Model>
    {
        internal FakeEditRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
