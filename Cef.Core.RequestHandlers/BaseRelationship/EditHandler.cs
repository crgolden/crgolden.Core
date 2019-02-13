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
    public abstract class EditHandler<T, T1, T2> : IRequestHandler<EditRequest<T, T1, T2>>
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        protected readonly DbContext Context;

        protected EditHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(EditRequest<T, T1, T2> request, CancellationToken cancellationToken = default)
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

            return default;
        }
    }
}
