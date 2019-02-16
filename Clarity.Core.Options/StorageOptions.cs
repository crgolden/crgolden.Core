namespace Clarity.Core
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class StorageOptions
    {
        public string ImageContainer { get; set; }

        public string ThumbnailContainer { get; set; }

        public AzureBlobStorageOptions AzureBlobStorageOptions { get; set; }
    }

    [PublicAPI]
    public class AzureBlobStorageOptions
    {
        public string AccountName { get; set; }

        public string AccountKey { get; set; }
    }
}