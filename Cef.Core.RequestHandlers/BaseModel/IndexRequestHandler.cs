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
    public abstract class IndexRequestHandler<TRequest, T> : IRequestHandler<TRequest, DataSourceResult>
        where TRequest : IndexRequest
        where T : BaseModel
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
