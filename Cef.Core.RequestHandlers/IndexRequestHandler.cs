namespace Cef.Core.RequestHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests;

    [PublicAPI]
    public abstract class IndexRequestHandler<TRequest, T> : IRequestHandler<TRequest, DataSourceResult>
        where TRequest : IndexRequest
        where T : Core.ModelBase
    {
        protected readonly DbContext Context;

        protected IndexRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<DataSourceResult> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>()
                .AsNoTracking()
                .ToDataSourceResultAsync(request.Request, request.ModelState)
                .ConfigureAwait(false);
        }
    }
}
