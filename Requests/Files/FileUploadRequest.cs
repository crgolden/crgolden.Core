namespace crgolden.Core.Files
{
    using System.Diagnostics.CodeAnalysis;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    [ExcludeFromCodeCoverage]
    public abstract class FileUploadRequest<TEntity, TModel> : IRequest<TModel[]>
        where TEntity : File, new()
        where TModel : FileModel
    {
        public readonly IFormFileCollection Files;

        public string ContainerName { get; set; }

        protected FileUploadRequest(IFormFileCollection files, string containerName)
        {
            Files = files;
            ContainerName = containerName;
        }
    }
}
