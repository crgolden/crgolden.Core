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
    public abstract class CreateHandler<T, T1, T2> : IRequestHandler<CreateRequest<T, T1, T2>, T>
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        protected readonly DbContext Context;

        protected CreateHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<T> Handle(CreateRequest<T, T1, T2> request, CancellationToken cancellationToken = default)
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
