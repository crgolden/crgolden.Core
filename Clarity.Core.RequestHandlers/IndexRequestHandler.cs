namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class IndexRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, DataSourceResult>
        where TRequest : IndexRequest
        where TResponse : class
    {
        protected readonly DbContext Context;

        protected IndexRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<DataSourceResult> Handle(TRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Context.Set<TResponse>()
                .AsNoTracking()
                .ToDataSourceResultAsync(request.Request, request.ModelState)
                .ConfigureAwait(false);
        }
    }
}
