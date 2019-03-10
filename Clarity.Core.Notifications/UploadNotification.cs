namespace Clarity.Core
{
    using System;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public abstract class UploadNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public IFormFileCollection Files { get; set; }

        public TModel[] Models { get; set; }

        public Exception Exception { get; set; }
    }
}
