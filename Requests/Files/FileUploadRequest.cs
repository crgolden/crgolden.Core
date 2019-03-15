namespace Clarity.Core.Files
{
    using System.Collections.Generic;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public abstract class FileUploadRequest<TEntity, TModel> : IRequest<IEnumerable<TModel>>
        where TEntity : File, new()
        where TModel : FileModel
    {
        public readonly IFormFileCollection Files;

        public string ContainerName { get; set; }

        protected FileUploadRequest(IFormFileCollection files)
        {
            Files = files;
        }
    }
}
