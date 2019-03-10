namespace Clarity.Core
{
    public static class FileExtensions
    {
        public static string GetImageFileUri(
            this File file,
            IStorageService storageService,
            StorageOptions options,
            bool thumbnail = false)
        {
            var containerName = thumbnail ? options.ThumbnailContainer : options.ImageContainer;
            var uri = thumbnail
                ? file.Uri.Replace($"{options.ImageContainer}/", $"{options.ThumbnailContainer}/")
                : file.Uri;
            var parts = file.Uri.Split('.');
            var fileName = $"{parts[parts.Length - 2]}{file.Extension}";
            var sharedAccessSignature = storageService.GetSharedAccessSignature(fileName, containerName);
            return $"{uri}{sharedAccessSignature}";
        }
    }
}
