namespace Clarity.Core
{
    using System.Collections.Generic;
    using MediatR;

    public abstract class CreateRangeRequest<TResponse, TEntity, TModel> : IRequest<TResponse>
        where TEntity : class
        where TResponse : IEnumerable<TModel>
    {
        public readonly IEnumerable<TEntity> Entities;

        protected CreateRangeRequest(IEnumerable<TEntity> entities)
        {
            Entities = entities;
        }
    }
}
