namespace Cef.Core.RequestHandlers.BaseRelationship
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.BaseRelationship;

    [PublicAPI]
    public abstract class CreateRequestHandler<TRequest, TResponse, T1, T2> : IRequestHandler<TRequest, TResponse>
        where TRequest : CreateRequest<TResponse, T1, T2>
        where TResponse : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        protected readonly DbContext Context;

        protected CreateRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            request.Relationship.Created = request.Relationship.Created > DateTime.MinValue
                ? request.Relationship.Created
                : DateTime.Now;
            Context.Add(request.Relationship);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return request.Relationship;
        }
    }
}
