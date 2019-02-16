namespace Clarity.Core
{
    using MediatR;

    public abstract class CreateRequest<TResponse> : IRequest<TResponse> where TResponse : class
    {
        public readonly TResponse Entity;

        protected CreateRequest(TResponse entity)
        {
            Entity = entity;
        }
    }
}
