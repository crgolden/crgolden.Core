namespace Clarity.Core
{
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public abstract class UploadRequest<TEntity, TModel> : IRequest<TModel[]>
        where TEntity : File
        where TModel: FileModel
    {
        public readonly IFormFileCollection Files;

        protected UploadRequest(IFormFileCollection files)
        {
            Files = files;
        }
    }
}
