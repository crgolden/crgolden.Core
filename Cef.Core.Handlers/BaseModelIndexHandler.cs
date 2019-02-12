namespace Cef.Core.Handlers
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
    public abstract class BaseModelIndexHandler<T> : IRequestHandler<BaseModelIndexRequest, DataSourceResult>
        where T : BaseModel
    {
        protected readonly DbContext Context;

        protected BaseModelIndexHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<DataSourceResult> Handle(BaseModelIndexRequest request, CancellationToken cancellationToken = default)
        {
            var models = Context.Set<T>().AsNoTracking();
            return await models.ToDataSourceResultAsync(
                request: request.Request,
                modelState: request.ModelState,
                selector: request.Selector);
        }
    }
}
