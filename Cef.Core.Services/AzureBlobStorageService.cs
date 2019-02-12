namespace Cef.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Options;

    [PublicAPI]
    public class AzureBlobStorageService : IStorageService
    {
        private readonly AzureBlobStorage _options;

        public AzureBlobStorageService(IOptions<StorageOptions> options)
        {
            _options = options.Value.AzureBlobStorage;
        }

        public async Task<Uri> UploadFileToStorageAsync(IFormFile file, string fileName)
        {
            using (var stream = file.OpenReadStream())
            {
                var storageCredentials = new StorageCredentials(
                    accountName: _options.AccountName,
                    keyValue: _options.AccountKey);
                var storageAccount = new CloudStorageAccount(
                    storageCredentials: storageCredentials,
                    useHttps: true);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(_options.ImageContainer);
                var blockBlob = container.GetBlockBlobReference(fileName);
                await blockBlob.UploadFromStreamAsync(stream);
                return blockBlob.Uri;
            }
        }

        public async Task<Uri> UploadByteArrayToStorageAsync(byte[] buffer, string fileName)
        {
            var storageCredentials = new StorageCredentials(
                accountName: _options.AccountName,
                keyValue: _options.AccountKey);
            var storageAccount = new CloudStorageAccount(
                storageCredentials: storageCredentials,
                useHttps: true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(_options.ImageContainer);
            var blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromByteArrayAsync(buffer, 0, buffer.Length);
            return blockBlob.Uri;
        }

        public string GetSharedAccessSignature(string fileName, string uri)
        {
            string containerName = null;
            if (uri.Contains(_options.ImageContainer))
            {
                containerName = _options.ImageContainer;
            }
            else if (uri.Contains(_options.ThumbnailContainer))
            {
                containerName = _options.ThumbnailContainer;
            }

            if (string.IsNullOrEmpty(containerName))
            {
                return null;
            }

            var storageCredentials = new StorageCredentials(
                accountName: _options.AccountName,
                keyValue: _options.AccountKey);
            var storageAccount = new CloudStorageAccount(
                storageCredentials: storageCredentials,
                useHttps: true);
            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(containerName);
            var blockBlob = container.GetBlockBlobReference(fileName);
            return blockBlob.GetSharedAccessSignature(new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(24),
                Permissions = SharedAccessBlobPermissions.Read
            });
        }

        public async Task DeleteAllFromStorageAsync()
        {
            var storageCredentials = new StorageCredentials(
                accountName: _options.AccountName,
                keyValue: _options.AccountKey);
            var storageAccount = new CloudStorageAccount(
                storageCredentials: storageCredentials,
                useHttps: true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var containerNames = new List<string>
            {
                _options.ImageContainer,
                _options.ThumbnailContainer
            };
            foreach (var containerName in containerNames)
            {
                foreach (var blob in blobClient
                    .GetContainerReference(containerName)
                    .ListBlobs(null, true)
                    .Where(x => x.GetType() == typeof(CloudBlob) || x.GetType().BaseType == typeof(CloudBlob)))
                {
                    
                    await ((CloudBlob)blob).DeleteIfExistsAsync();
                }
            }
        }
    }
}
