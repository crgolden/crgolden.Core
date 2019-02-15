namespace Cef.Core.RequestHandlers.RelationshipBase
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.RelationshipBase;

    [PublicAPI]
    public abstract class DetailsRequestHandler<TRequest, TResponse, T1, T2> : IRequestHandler<TRequest, TResponse>
        where TRequest : DetailsRequest<TResponse, T1, T2>
        where TResponse : RelationshipBase<T1, T2>
        where T1 : ModelBase
        where T2 : ModelBase
    {
        protected readonly DbContext Context;

        protected DetailsRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            return await Context.FindAsync<TResponse>(request.Id1, request.Id2).ConfigureAwait(false);
        }
    }
}
