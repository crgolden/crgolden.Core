namespace Clarity.Core
{
    using MediatR;

    public abstract class EditRequest<T> : IRequest where T : class
    {
        public readonly T Entity;

        protected EditRequest(T entity)
        {
            Entity = entity;
        }
    }
}
