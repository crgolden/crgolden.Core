namespace Clarity.Core
{
    using System;
    using System.Collections.Generic;
    using MediatR;

    public abstract class EditRangeNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public IEnumerable<TModel> Models { get; set; }

        public Exception Exception { get; set; }
    }
}
