namespace Cef.Core.RequestHandlers.BaseModel
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.BaseModel;

    [PublicAPI]
    public abstract class DeleteRequestHandler<TRequest, T> : IRequestHandler<TRequest>
        where TRequest : DeleteRequest
        where T : BaseModel
    {
        protected readonly DbContext Context;

        protected DeleteRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            var model = await Context.FindAsync<T>(request.Id);
            if (model == null) return default;

            Context.Remove(model);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return default;
        }
    }
}
