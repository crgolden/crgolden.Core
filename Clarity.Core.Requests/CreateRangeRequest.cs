namespace Clarity.Core
{
    using System.Collections.Generic;
    using MediatR;

    public abstract class CreateRangeRequest<TResponse, TEntity, TModel> : IRequest<TResponse>
        where TEntity : Entity
        where TModel : Model
        where TResponse : IEnumerable<TModel>
    {
        public readonly IEnumerable<TModel> Models;

        protected CreateRangeRequest(IEnumerable<TModel> models)
        {
            Models = models;
        }
    }
}
