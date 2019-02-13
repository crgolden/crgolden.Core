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
    public abstract class DetailsHandler<T> : IRequestHandler<DetailsRequest<T>, T>
        where T : BaseModel
    {
        protected readonly DbContext Context;

        protected DetailsHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<T> Handle(DetailsRequest<T> request, CancellationToken cancellationToken = default)
        {
            return await Context.FindAsync<T>(request.Id).ConfigureAwait(false);
        }
    }
}
