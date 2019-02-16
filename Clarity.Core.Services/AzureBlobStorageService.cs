namespace Clarity.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;

    [PublicAPI]
    public class AzureBlobStorageService : IStorageService
    {
        private readonly string _imagesContainer;
        private readonly string _thumbnailsContainer;
        private readonly CloudBlobClient _client;

        public AzureBlobStorageService(IOptions<StorageOptions> options)
        {
            _imagesContainer = options.Value.ImageContainer;
            _thumbnailsContainer = options.Value.ThumbnailContainer;
            var storageCredentials = new StorageCredentials(
                accountName: options.Value.AzureBlobStorageOptions.AccountName,
                keyValue: options.Value.AzureBlobStorageOptions.AccountKey);
            var storageAccount = new CloudStorageAccount(
                storageCredentials: storageCredentials,
                useHttps: true);
            _client = storageAccount.CreateCloudBlobClient();
        }

        public virtual async Task<Uri> UploadFileToStorageAsync(IFormFile file, string fileName)
        {
            var container = _client.GetContainerReference(_imagesContainer);
            var blockBlob = container.GetBlockBlobReference(fileName);
            using (var stream = file.OpenReadStream())
            {
                await blockBlob.UploadFromStreamAsync(stream).ConfigureAwait(false);
                return blockBlob.Uri;
            }
        }

        public virtual async Task<Uri> UploadByteArrayToStorageAsync(byte[] buffer, string fileName)
        {
            var container = _client.GetContainerReference(_imagesContainer);
            var blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromByteArrayAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            return blockBlob.Uri;
        }

        public virtual string GetSharedAccessSignature(string fileName, string uri)
        {
            string containerName = null;
            if (uri.Contains(_imagesContainer))
            {
                containerName = _imagesContainer;
            }
            else if (uri.Contains(_thumbnailsContainer))
            {
                containerName = _thumbnailsContainer;
            }

            if (string.IsNullOrEmpty(containerName))
            {
                return null;
            }

            var container = _client.GetContainerReference(containerName);
            var blockBlob = container.GetBlockBlobReference(fileName);
            var policy = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(24),
                Permissions = SharedAccessBlobPermissions.Read
            };
            return blockBlob.GetSharedAccessSignature(policy);
        }

        public virtual async Task DeleteAllFromStorageAsync()
        {
            var containerNames = new List<string>
            {
                _imagesContainer,
                _thumbnailsContainer
            };
            foreach (var containerName in containerNames)
            {
                foreach (var blob in _client
                    .GetContainerReference(containerName)
                    .ListBlobs(null, true)
                    .Where(x => x.GetType() == typeof(CloudBlob) || x.GetType().BaseType == typeof(CloudBlob)))
                {
                    
                    await ((CloudBlob)blob).DeleteIfExistsAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
