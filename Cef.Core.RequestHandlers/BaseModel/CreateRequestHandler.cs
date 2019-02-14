﻿namespace Cef.Core.RequestHandlers.BaseModel
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.BaseModel;

    [PublicAPI]
    public abstract class CreateRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : CreateRequest<TResponse>
        where TResponse : BaseModel
    {
        protected readonly DbContext Context;

        protected CreateRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            request.Model.Created = request.Model.Created > DateTime.MinValue
                ? request.Model.Created
                : DateTime.Now;
            Context.Add(request.Model);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return request.Model;
        }
    }
}
