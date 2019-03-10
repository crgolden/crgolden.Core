namespace Clarity.Core
{
    using MediatR;

    public abstract class CreateRequest<TEntity, TModel> : IRequest<TModel>
        where TEntity : Entity
        where TModel : Model
    {
        public readonly TModel Model;

        protected CreateRequest(TModel model)
        {
            Model = model;
        }
    }
}
