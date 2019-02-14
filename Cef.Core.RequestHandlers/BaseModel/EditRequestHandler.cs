namespace Cef.Core.RequestHandlers.BaseModel
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.BaseModel;

    [PublicAPI]
    public abstract class EditRequestHandler<TRequest, T> : IRequestHandler<TRequest>
        where TRequest : EditRequest<T>
        where T : BaseModel
    {
        protected readonly DbContext Context;

        protected EditRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            request.Model.Updated = request.Model.Updated ?? DateTime.UtcNow;
            Context.Entry(request.Model).State = EntityState.Modified;
            try
            {
                await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                var model = await Context.FindAsync<T>(request.Model.Id).ConfigureAwait(false);
                if (model != null)
                {
                    throw;
                }
            }

            return default;
        }
    }
}
