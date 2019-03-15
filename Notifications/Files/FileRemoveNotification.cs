namespace Clarity.Core.Files
{
    using System;
    using System.Collections.Generic;
    using MediatR;

    public abstract class FileRemoveNotification<TKey> : INotification
    {
        public EventIds EventId { get; set; }

        public IEnumerable<string> FileNames { get; set; }

        public TKey[][] KeyValues { get; set; }

        public Exception Exception { get; set; }
    }
}
