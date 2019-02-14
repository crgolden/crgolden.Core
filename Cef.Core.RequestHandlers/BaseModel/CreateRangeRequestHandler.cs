namespace Cef.Core.RequestHandlers.BaseModel
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.BaseModel;

    [PublicAPI]
    public abstract class CreateRangeRequestHandler<TRequest, TResponse, TModel> : IRequestHandler<TRequest, TResponse>
        where TRequest : CreateRangeRequest<TResponse, TModel>
        where TResponse : IEnumerable<TModel>
        where TModel : BaseModel
    {
        protected readonly DbContext Context;

        protected CreateRangeRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            var created = DateTime.UtcNow;
            foreach (var model in request.Models)
            {
                model.Created = model.Created > DateTime.MinValue
                    ? model.Created
                    : created;
                Context.Add(model);
            }

            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return request.Models;
        }
    }
}
