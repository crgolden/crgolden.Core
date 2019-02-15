namespace Cef.Core.RequestHandlers.RelationshipBase
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.RelationshipBase;

    [PublicAPI]
    public abstract class DeleteRequestHandler<TRequest, T, T1, T2> : IRequestHandler<TRequest>
        where TRequest : DeleteRequest
        where T : RelationshipBase<T1, T2>
        where T1 : ModelBase
        where T2 : ModelBase
    {
        protected readonly DbContext Context;

        protected DeleteRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            var relationship = await Context.FindAsync<T>(request.Id1, request.Id2).ConfigureAwait(false);
            if (relationship == null) return Unit.Value;

            Context.Remove(relationship);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
