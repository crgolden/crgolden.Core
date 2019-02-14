namespace Cef.Core.RequestHandlers.BaseModel
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.BaseModel;

    [PublicAPI]
    public abstract class EditRangeRequestHandler<T> : IRequestHandler<EditRangeRequest<T>>
        where T : BaseModel
    {
        protected readonly DbContext Context;

        protected EditRangeRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(EditRangeRequest<T> request, CancellationToken cancellationToken = default)
        {
            var updated = DateTime.UtcNow;
            foreach (var model in request.Models)
            {
                model.Updated = model.Updated ?? updated;
                Context.Entry(model).State = EntityState.Modified;
            }

            try
            {
                await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException e)
            {
                foreach (var entity in e.Entries.Select(x => x.Entity).Cast<Core.BaseModel>())
                {
                    var model = await Context.FindAsync<T>(entity.Id).ConfigureAwait(false);
                    if (model != null)
                    {
                        throw;
                    }
                }
            }

            return default;
        }
    }
}
