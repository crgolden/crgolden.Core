namespace Cef.Core.RequestHandlers.BaseModel
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.BaseModel;

    [PublicAPI]
    public abstract class IndexRequestHandler<T> : IRequestHandler<IndexRequest<T>, DataSourceResult>
        where T : BaseModel
    {
        protected readonly DbContext Context;

        protected IndexRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<DataSourceResult> Handle(IndexRequest<T> request, CancellationToken cancellationToken = default)
        {
            var models = Context.Set<T>().AsNoTracking();
            return await models.ToDataSourceResultAsync(
                request: request.Request,
                modelState: request.ModelState,
                selector: request.Selector).ConfigureAwait(false);
        }
    }
}
