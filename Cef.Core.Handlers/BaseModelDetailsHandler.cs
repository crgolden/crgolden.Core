namespace Cef.Core.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests;

    [PublicAPI]
    public abstract class BaseModelDetailsHandler<T> : IRequestHandler<BaseModelDetailsRequest<T>, T>
        where T : BaseModel
    {
        protected readonly DbContext Context;

        protected BaseModelDetailsHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<T> Handle(BaseModelDetailsRequest<T> request, CancellationToken cancellationToken = default)
        {
            return await Context.FindAsync<T>(request.Id);
        }
    }
}
