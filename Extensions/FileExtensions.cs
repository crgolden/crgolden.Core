namespace crgolden.Core
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Shared;

    [ExcludeFromCodeCoverage]
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
            var fileName = file.Uri.Split('/').Last();
            var sharedAccessSignature = storageService.GetSharedAccessSignature(fileName, containerName);
            return $"{uri}{sharedAccessSignature}";
        }
    }
}
