namespace Clarity.Core
{
    using System;
    using System.Collections.Generic;
    using MediatR;

    public abstract class CreateRangeNotification<TModel> : INotification
        where TModel : Model
    {
        public EventIds EventId { get; set; }

        public IEnumerable<TModel> Models { get; set; }

        public Exception Exception { get; set; }
    }
}
