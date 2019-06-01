namespace crgolden.Core.Files
{
    using System;
    using MediatR;
    using Shared;

    public abstract class FileRemoveNotification<TKey> : INotification
    {
        public EventIds EventId { get; set; }

        public string[] FileNames { get; set; }

        public TKey[][] KeyValues { get; set; }

        public Exception Exception { get; set; }
    }
}
