namespace crgolden.Core.Files
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Shared;

    public abstract class FileUploadRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, TModel[]>
        where TRequest : FileUploadRequest<TEntity, TModel>
        where TEntity : File, new()
        where TModel : FileModel
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;
        protected readonly IStorageService StorageService;

        protected FileUploadRequestHandler(DbContext context, IMapper mapper, IStorageService storageService)
        {
            Context = context;
            Mapper = mapper;
            StorageService = storageService;
        }

        public virtual async Task<TModel[]> Handle(TRequest request, CancellationToken token)
        {
            if (request.Files.Count == 0) return new TModel[0];
            var tasks = request.Files.Select(async formFile =>
            {
                var uri = await StorageService.UploadFileToStorageAsync(
                    file: formFile,
                    containerName: request.ContainerName,
                    token: token).ConfigureAwait(false);
                var index = formFile.FileName.LastIndexOf('.');
                return new TEntity
                {
                    ContentType = formFile.ContentType,
                    Extension = formFile.FileName.Substring(index),
                    Name = formFile.FileName,
                    Uri = $"{uri}",
                    Size = formFile.Length
                };
            });
            var files = await Task.WhenAll(tasks).ConfigureAwait(false);
            Context.Set<TEntity>().AddRange(files);
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Mapper.Map<TModel[]>(files);
        }
    }
}
