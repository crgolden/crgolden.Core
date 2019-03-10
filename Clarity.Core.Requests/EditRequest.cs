namespace Clarity.Core
{
    using MediatR;

    public abstract class EditRequest<TEntity, TModel> : IRequest
        where TEntity : Entity
        where TModel : Model
    {
        public readonly TModel Model;

        protected EditRequest(TModel model)
        {
            Model = model;
        }
    }
}
