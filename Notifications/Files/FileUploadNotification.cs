namespace Clarity.Core.Files
{
    using System;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Shared;

    public abstract class FileUploadNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public IFormFileCollection Files { get; set; }

        public TModel[] Models { get; set; }

        public Exception Exception { get; set; }
    }
}
