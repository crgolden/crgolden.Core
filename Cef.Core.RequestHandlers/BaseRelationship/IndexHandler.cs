﻿namespace Cef.Core.RequestHandlers.BaseRelationship
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.BaseRelationship;

    [PublicAPI]
    public abstract class IndexHandler<T, T1, T2> : IRequestHandler<IndexRequest<T, T1, T2>, DataSourceResult>
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        protected readonly DbContext Context;

        protected IndexHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<DataSourceResult> Handle(IndexRequest<T, T1, T2> request, CancellationToken cancellationToken = default)
        {
            var models = Context.Set<T>().AsNoTracking();
            return await models.ToDataSourceResultAsync(
                request: request.Request,
                modelState: request.ModelState,
                selector: request.Selector).ConfigureAwait(false);
        }
    }
}
