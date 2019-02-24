﻿namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class DetailsRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, TModel>
        where TRequest : DetailsRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;

        protected DetailsRequestHandler(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual async Task<TModel> Handle(TRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entity = await Context
                .FindAsync<TEntity>(request.KeyValues, cancellationToken)
                .ConfigureAwait(false);
            return Mapper.Map<TModel>(entity);
        }
    }
}
