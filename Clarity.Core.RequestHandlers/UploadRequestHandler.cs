namespace Clarity.Core
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class UploadRequestHandler<TEntity, TModel> : IRequestHandler<UploadRequest<TEntity, TModel>, TModel[]>
        where TEntity : File, new()
        where TModel : FileModel
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;
        protected readonly IStorageService StorageService;

        protected UploadRequestHandler(DbContext context, IMapper mapper, IStorageService storageService)
        {
            Context = context;
            Mapper = mapper;
            StorageService = storageService;
        }

        public virtual async Task<TModel[]> Handle(UploadRequest<TEntity, TModel> request, CancellationToken token)
        {
            if (request.Files.Count == 0) return new TModel[0];
            var tasks = request.Files.Select(async (x, i) =>
            {
                var index = x.FileName.LastIndexOf('.');
                var extension = x.FileName.Substring(index);
                var uri = await StorageService.UploadFileToStorageAsync(
                    file: x,
                    fileName: $"{Guid.NewGuid()}{extension}",
                    token: token).ConfigureAwait(false);
                return new TEntity
                {
                    ContentType = x.ContentType,
                    Extension = extension,
                    Name = x.FileName,
                    Uri = $"{uri}",
                    Size = x.Length
                };
            });
            var files = await Task.WhenAll(tasks).ConfigureAwait(false);
            Context.Set<TEntity>().AddRange(files.Cast<TEntity>());
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Mapper.Map<TModel[]>(files);
        }
    }
}
