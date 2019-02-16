namespace Clarity.Core
{
    using System.Collections.Generic;
    using MediatR;

    public abstract class CreateRangeRequest<TResponse, T> : IRequest<TResponse>
        where T : class
        where TResponse : IEnumerable<T>
    {
        public readonly TResponse Entities;

        protected CreateRangeRequest(TResponse entities)
        {
            Entities = entities;
        }
    }
}
