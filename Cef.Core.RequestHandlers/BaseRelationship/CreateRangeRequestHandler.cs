namespace Cef.Core.RequestHandlers.BaseRelationship
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.BaseRelationship;

    [PublicAPI]
    public abstract class CreateRangeRequestHandler<T, TRelationship, T1, T2> : IRequestHandler<CreateRangeRequest<T, TRelationship, T1, T2>, T>
        where T : IEnumerable<TRelationship>
        where TRelationship : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        protected readonly DbContext Context;

        protected CreateRangeRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<T> Handle(CreateRangeRequest<T, TRelationship, T1, T2> request, CancellationToken cancellationToken = default)
        {
            var created = DateTime.UtcNow;
            foreach (var relationship in request.Relationships)
            {
                relationship.Created = relationship.Created > DateTime.MinValue
                    ? relationship.Created
                    : created;
                Context.Add(relationship);
            }

            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return request.Relationships;
        }
    }
}
