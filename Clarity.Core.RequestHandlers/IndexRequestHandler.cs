﻿namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class IndexRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, DataSourceResult>
        where TRequest : IndexRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;

        protected IndexRequestHandler(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual async Task<DataSourceResult> Handle(TRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entities = Context.Set<TEntity>().AsNoTracking();
            return await Mapper
                .ProjectTo<TModel>(entities)
                .ToDataSourceResultAsync(request.Request, request.ModelState)
                .ConfigureAwait(false);
        }
    }
}
