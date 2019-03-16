namespace Clarity.Core.Files
{
    using MediatR;

    public abstract class FileRemoveRequest<TKey> : IRequest
    {
        public readonly string[] FileNames;

        public readonly TKey[][] Keys;

        public readonly string ContainerName;

        public readonly string ThumbnailContainerName;

        protected FileRemoveRequest(
            string[] fileNames,
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
