namespace Cef.Core.RequestHandlers.BaseRelationship
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.BaseRelationship;

    [PublicAPI]
    public abstract class DetailsRequestHandler<T, T1, T2> : IRequestHandler<DetailsRequest<T, T1, T2>, T>
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        protected readonly DbContext Context;

        protected DetailsRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<T> Handle(DetailsRequest<T, T1, T2> request, CancellationToken cancellationToken = default)
        {
            return await Context.FindAsync<T>(request.Id1, request.Id2).ConfigureAwait(false);
        }
    }
}
