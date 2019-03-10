namespace Clarity.Core
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public abstract class RemoveRequestHandler<TEntity, TKey> : IRequestHandler<RemoveRequest<TKey>>
        where TEntity : File
    {
        protected readonly DbContext Context;
        protected readonly IStorageService StorageService;
        protected readonly string Images;
        protected readonly string Thumbnails;

        protected RemoveRequestHandler(
            DbContext context,
            IStorageService storageService,
            IOptions<StorageOptions> storageOptions)
        {
            Context = context;
            StorageService = storageService;
            Images = storageOptions.Value.ImageContainer;
            Thumbnails = storageOptions.Value.ThumbnailContainer;
        }

        public virtual async Task<Unit> Handle(RemoveRequest<TKey> request, CancellationToken token)
        {
            if (request.FileNames.Length == 0) return Unit.Value;
            for (var i = 0; i < request.FileNames.Length; i++)
            {
                if (request.Keys.Length > i)
                {
                    await RemoveById(request.Keys[i].Cast<object>().ToArray(), token).ConfigureAwait(false);
                }
                else
                {
                    await RemoveByName(request.FileNames[i], token).ConfigureAwait(false);
                }
            }

            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Unit.Value;
        }

        private async Task RemoveById(object[] keyValues, CancellationToken token)
        {
            var file = await Context.FindAsync<TEntity>(keyValues, token).ConfigureAwait(false);
            if (file == null) return;
            await RemoveFromStorage(file, token).ConfigureAwait(false);
        }

        private async Task RemoveByName(string name, CancellationToken token)
        {
            var files = Context.Set<TEntity>().Where(x => x.Name == name);
            if (!await files.AnyAsync(token)) return;
            foreach (var file in files)
            {
                await RemoveFromStorage(file, token).ConfigureAwait(false);
            }
        }

        private async Task RemoveFromStorage(TEntity file, CancellationToken token)
        {
            var deleteThumbnails = !string.IsNullOrEmpty(Thumbnails);
            await StorageService
                .DeleteFileFromStorageAsync(
                    token: token,
                    blobUri: new Uri(file.Uri))
                .ConfigureAwait(false);
            Context.Remove(file);
            if (!deleteThumbnails) return;
            await StorageService
                .DeleteFileFromStorageAsync(
                    token: token,
                    blobUri: new Uri(file.Uri.Replace(Images, Thumbnails)))
                .ConfigureAwait(false);
        }
    }
}
