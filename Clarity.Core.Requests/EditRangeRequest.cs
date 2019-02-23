namespace Clarity.Core
{
    using System.Collections.Generic;
    using MediatR;

    public abstract class EditRangeRequest<TEntity> : IRequest
        where TEntity : class
    {
        public readonly IEnumerable<TEntity> Entities;

        protected EditRangeRequest(IEnumerable<TEntity> entities)
        {
            Entities = entities;
        }
    }
}
