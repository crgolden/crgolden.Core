namespace Clarity.Core
{
    using MediatR;

    public abstract class CreateRequest<TEntity, TModel> : IRequest<TModel>
        where TEntity : class
    {
        public readonly TEntity Entity;

        protected CreateRequest(TEntity entity)
        {
            Entity = entity;
        }
    }
}
