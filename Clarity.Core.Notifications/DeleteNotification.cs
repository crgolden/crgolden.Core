namespace Clarity.Core
{
    using System;
    using MediatR;

    public abstract class DeleteNotification : INotification
    {
        public EventIds EventId { get; set; }

        public object[] KeyValues { get; set; }

        public Exception Exception { get; set; }
    }
}
