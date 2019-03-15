namespace Clarity.Core.Files
{
    using System.Collections.Generic;
    using MediatR;

    public abstract class FileRemoveRequest<TKey> : IRequest
    {
        public readonly IEnumerable<string> FileNames;

        public readonly TKey[][] Keys;

        public readonly string ContainerName;

        public readonly string ThumbnailContainerName;

        protected FileRemoveRequest(
            IEnumerable<string> fileNames,
            string containerName,
            string thumbnailContainerName = null,
            TKey[][] keys = null)
        {
            FileNames = fileNames;
            Keys = keys;
            ContainerName = containerName;
            ThumbnailContainerName = thumbnailContainerName;
        }
    }
}
