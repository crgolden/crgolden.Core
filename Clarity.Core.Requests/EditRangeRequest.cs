namespace Clarity.Core
{
    using System.Collections.Generic;
    using MediatR;

    public abstract class EditRangeRequest<T> : IRequest where T : class
    {
        public readonly IEnumerable<T> Entities;

        protected EditRangeRequest(IEnumerable<T> entities)
        {
            Entities = entities;
        }
    }
}
