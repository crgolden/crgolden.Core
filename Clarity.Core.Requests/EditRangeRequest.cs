namespace Clarity.Core
{
    using System.Collections.Generic;
    using MediatR;

    public abstract class EditRangeRequest<TEntity, TModel> : IRequest
        where TEntity : Entity
        where TModel : Model
    {
        public readonly IEnumerable<TModel> Models;

        protected EditRangeRequest(IEnumerable<TModel> models)
        {
            Models = models;
        }
    }
}
