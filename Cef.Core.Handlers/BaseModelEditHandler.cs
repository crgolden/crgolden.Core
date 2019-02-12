namespace Cef.Core.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests;

    [PublicAPI]
    public abstract class BaseModelEditHandler<T> : IRequestHandler<BaseModelEditRequest<T>, T>
        where T : BaseModel
    {
        protected readonly DbContext Context;

        protected BaseModelEditHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<T> Handle(BaseModelEditRequest<T> request, CancellationToken cancellationToken = default)
        {
            request.Model.Updated = request.Model.Updated ?? DateTime.Now;
            Context.Entry(request.Model).State = EntityState.Modified;
            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await Context.FindAsync<T>(request.Model.Id) != null)
                {
                    throw;
                }
            }

            return request.Model;
        }
    }
}
