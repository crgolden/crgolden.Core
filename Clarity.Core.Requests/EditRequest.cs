namespace Clarity.Core
{
    using MediatR;

    public abstract class EditRequest<TEntity> : IRequest
        where TEntity : class
    {
        public readonly TEntity Entity;

        protected EditRequest(TEntity entity)
        {
            Entity = entity;
        }
    }
}
