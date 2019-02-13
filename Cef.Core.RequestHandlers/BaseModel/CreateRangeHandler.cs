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
    public abstract class CreateRangeHandler<T, TModel> : IRequestHandler<CreateRangeRequest<T, TModel>, T>
        where T : IEnumerable<TModel>
        where TModel : BaseModel
    {
        protected readonly DbContext Context;

        protected CreateRangeHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<T> Handle(CreateRangeRequest<T, TModel> request, CancellationToken cancellationToken = default)
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
