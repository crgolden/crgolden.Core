namespace Cef.Core.RequestHandlers.BaseModel
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
    public abstract class CreateHandler<T> : IRequestHandler<CreateRequest<T>, T>
        where T : BaseModel
    {
        protected readonly DbContext Context;

        protected CreateHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<T> Handle(CreateRequest<T> request, CancellationToken cancellationToken = default)
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
