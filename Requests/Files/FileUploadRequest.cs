namespace Clarity.Core.Files
{
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public abstract class FileUploadRequest<TEntity, TModel> : IRequest<TModel[]>
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
