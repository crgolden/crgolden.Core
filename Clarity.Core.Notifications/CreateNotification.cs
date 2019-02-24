namespace Clarity.Core
{
    using System;
    using MediatR;

    public abstract class CreateNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public TModel Model { get; set; }

        public Exception Exception { get; set; }
    }
}
