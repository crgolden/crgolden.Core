namespace crgolden.Core
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Auth;
    using Microsoft.Azure.Storage.Blob;
    using Microsoft.Extensions.Options;
    using Shared;

    public class AzureBlobStorageService : IStorageService
    {
        private readonly CloudBlobClient _client;

        public AzureBlobStorageService(IOptions<StorageOptions> options)
        {
            var storageCredentials = new StorageCredentials(
                accountName: options.Value.AzureBlobStorageOptions.AccountName,
                keyValue: options.Value.AzureBlobStorageOptions.AccountKey1);
            var storageAccount = new CloudStorageAccount(
                storageCredentials: storageCredentials,
                useHttps: true);
            _client = storageAccount.CreateCloudBlobClient();
        }

        public virtual async Task<Uri> UploadStreamToStorageAsync(
            Stream stream,
            string fileName,
            string containerName,
            CancellationToken token)
        {
            var container = _client.GetContainerReference(containerName);
            var blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromStreamAsync(stream, token).ConfigureAwait(false);
            return blockBlob.Uri;
        }

        public virtual async Task<Uri> UploadByteArrayToStorageAsync(
            byte[] buffer,
            string fileName,
            string containerName,
            CancellationToken token)
        {
            var container = _client.GetContainerReference(containerName);
            var blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromByteArrayAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);
            return blockBlob.Uri;
        }

        public virtual string GetSharedAccessSignature(
            string fileName,
            string containerName)
        {
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

        public virtual async Task<bool> DeleteFileFromStorageAsync(Uri blobUri, CancellationToken token)
        {
            var blob = await _client.GetBlobReferenceFromServerAsync(blobUri, token).ConfigureAwait(false);
            return blob is CloudBlob clodBlob && await clodBlob.DeleteIfExistsAsync(token).ConfigureAwait(false);
        }

        public virtual async Task DeleteAllFromStorageAsync(string containerName, CancellationToken token)
        {
            foreach (var blob in _client
                .GetContainerReference(containerName)
                .ListBlobs(null, true)
                .Where(x => x.GetType() == typeof(CloudBlob) || x.GetType().BaseType == typeof(CloudBlob)))
            {
                await ((CloudBlob) blob).DeleteIfExistsAsync(token).ConfigureAwait(false);
            }
        }
    }
}
