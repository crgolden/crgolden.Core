namespace Clarity.Core
{
    using System;
    using MediatR;

    public abstract class RemoveNotification<TKey> : INotification
    {
        public EventIds EventId { get; set; }

        public string[] FileNames { get; set; }

        public TKey[][] KeyValues { get; set; }

        public Exception Exception { get; set; }
    }
}
