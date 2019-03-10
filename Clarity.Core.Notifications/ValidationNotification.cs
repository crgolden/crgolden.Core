namespace Clarity.Core
{
    using System;
    using MediatR;

    public abstract class ValidateNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public TModel Model { get; set; }

        public bool Valid { get; set; } = true;

        public Exception Exception { get; set; }
    }
}
