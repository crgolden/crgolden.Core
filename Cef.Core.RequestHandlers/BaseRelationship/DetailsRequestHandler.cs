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
    public abstract class DetailsRequestHandler<TRequest, TResponse, T1, T2> : IRequestHandler<TRequest, TResponse>
        where TRequest : DetailsRequest<TResponse, T1, T2>
        where TResponse : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
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
