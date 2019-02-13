﻿namespace Cef.Core.RequestHandlers.BaseRelationship
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.BaseRelationship;

    [PublicAPI]
    public abstract class DeleteHandler<T, T1, T2> : IRequestHandler<DeleteRequest>
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        protected readonly DbContext Context;

        protected DeleteHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(DeleteRequest request, CancellationToken cancellationToken = default)
        {
            var relationship = await Context.FindAsync<T>(request.Id1, request.Id2);
            if (relationship == null) return default;

            Context.Remove(relationship);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return default;
        }
    }
}