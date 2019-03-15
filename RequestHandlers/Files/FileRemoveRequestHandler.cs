namespace Clarity.Core.Files
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Shared;

    public abstract class FileRemoveRequestHandler<TRequest, TEntity, TKey> : IRequestHandler<TRequest>
        where TRequest : FileRemoveRequest<TKey>
        where TEntity : File
    {
        protected readonly DbContext Context;
        protected readonly IStorageService StorageService;

        protected FileRemoveRequestHandler(DbContext context, IStorageService storageService)
        {
            Context = context;
            StorageService = storageService;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken token)
        {
            if (!request.FileNames.Any()) return Unit.Value;
            var fileNames = request.FileNames.ToArray();
            for (var i = 0; i < fileNames.Length; i++)
            {
                if (request.Keys.Length > i)
                {
                    await RemoveById(
                        request.Keys[i].Cast<object>().ToArray(),
                        request.ContainerName,
                        request.ThumbnailContainerName,
                        token).ConfigureAwait(false);
                }
                else
                {
                    await RemoveByName(
                        fileNames[i],
                        request.ContainerName,
                        request.ThumbnailContainerName,
                        token).ConfigureAwait(false);
                }
            }

            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Unit.Value;
        }

        private async Task RemoveById(
            object[] keyValues,
            string containerName,
            string thumbnailsName,
            CancellationToken token)
        {
            var file = await Context.FindAsync<TEntity>(keyValues, token).ConfigureAwait(false);
            if (file == null) return;
            await RemoveFromStorage(file, containerName, thumbnailsName, token).ConfigureAwait(false);
        }

        private async Task RemoveByName(
            string name,
            string containerName,
            string thumbnailsName,
            CancellationToken token)
        {
            var files = Context.Set<TEntity>().Where(x => x.Name == name);
            if (!await files.AnyAsync(token)) return;
            foreach (var file in files)
            {
                await RemoveFromStorage(file, containerName, thumbnailsName, token).ConfigureAwait(false);
            }
        }

        private async Task RemoveFromStorage(
            TEntity file,
            string containerName,
            string thumbnailsName,
            CancellationToken token)
        {
            await StorageService
                .DeleteFileFromStorageAsync(
                    token: token,
                    blobUri: new Uri(file.Uri))
                .ConfigureAwait(false);
            Context.Remove(file);
            if (string.IsNullOrEmpty(thumbnailsName)) return;
            await StorageService
                .DeleteFileFromStorageAsync(
                    token: token,
                    blobUri: new Uri(file.Uri.Replace(containerName, thumbnailsName)))
                .ConfigureAwait(false);
        }
    }
}
