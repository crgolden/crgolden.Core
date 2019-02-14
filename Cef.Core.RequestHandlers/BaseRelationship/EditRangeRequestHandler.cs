namespace Cef.Core.RequestHandlers.BaseRelationship
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.BaseRelationship;

    [PublicAPI]
    public abstract class EditRangeRequestHandler<TRequest, T, T1, T2> : IRequestHandler<TRequest>
        where TRequest : EditRangeRequest<T, T1, T2>
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        protected readonly DbContext Context;

        protected EditRangeRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            var updated = DateTime.UtcNow;
            foreach (var relationship in request.Relationships)
            {
                relationship.Updated = relationship.Updated ?? updated;
                Context.Entry(relationship).State = EntityState.Modified;
            }

            try
            {
                await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException e)
            {
                foreach (var entity in e.Entries.Select(x => x.Entity).Cast<BaseRelationship<T1, T2>>())
                {
                    var relationship = await Context.FindAsync<T>(entity.Model1Id, entity.Model2Id).ConfigureAwait(false);
                    if (relationship != null)
                    {
                        throw;
                    }
                }
            }

            return default;
        }
    }
}
