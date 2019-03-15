namespace Clarity.Core.Files
{
    using System;
    using System.Collections.Generic;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public abstract class FileUploadNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public IFormFileCollection Files { get; set; }

        public IEnumerable<TModel> Models { get; set; }

        public Exception Exception { get; set; }
    }
}
