namespace Cef.Core.RequestHandlers.RelationshipBase
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.RelationshipBase;

    [PublicAPI]
    public abstract class EditRequestHandler<TRequest, T, T1, T2> : IRequestHandler<TRequest>
        where TRequest : EditRequest<T, T1, T2>
        where T : RelationshipBase<T1, T2>
        where T1 : ModelBase
        where T2 : ModelBase
    {
        protected readonly DbContext Context;

        protected EditRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            request.Relationship.Updated = request.Relationship.Updated ?? DateTime.UtcNow;
            Context.Entry(request.Relationship).State = EntityState.Modified;
            try
            {
                await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                var relationship = await Context.FindAsync<T>(request.Relationship.Model1Id, request.Relationship.Model2Id).ConfigureAwait(false);
                if (relationship != null)
                {
                    throw;
                }
            }

            return Unit.Value;
        }
    }
}
