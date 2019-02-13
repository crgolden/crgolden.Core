namespace Cef.Core.Options
{
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class StorageOptions
    {
        public string ImageContainer { get; set; }

        public string ThumbnailContainer { get; set; }

        public AzureBlobStorage AzureBlobStorage { get; set; }
    }

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class AzureBlobStorage
    {
        public string AccountName { get; set; }

        public string AccountKey { get; set; }
    }
}