namespace Clarity.Core
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class AzureBlobStorageService : IStorageService
    {
        private readonly string _imagesContainer;
        private readonly CloudBlobClient _client;

        public AzureBlobStorageService(IOptions<StorageOptions> options)
        {
            _imagesContainer = options.Value.ImageContainer;
            var storageCredentials = new StorageCredentials(
                accountName: options.Value.AzureBlobStorageOptions.AccountName,
                keyValue: options.Value.AzureBlobStorageOptions.AccountKey);
            var storageAccount = new CloudStorageAccount(
                storageCredentials: storageCredentials,
                useHttps: true);
            _client = storageAccount.CreateCloudBlobClient();
        }

        public virtual async Task<Uri> UploadFileToStorageAsync(
            IFormFile file,
            string fileName,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var container = _client.GetContainerReference(_imagesContainer);
            var blockBlob = container.GetBlockBlobReference(fileName);
            using (var stream = file.OpenReadStream())
            {
                await blockBlob.UploadFromStreamAsync(stream, token).ConfigureAwait(false);
                return blockBlob.Uri;
            }
        }

        public virtual async Task<Uri> UploadByteArrayToStorageAsync(
            byte[] buffer,
            string fileName,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var container = _client.GetContainerReference(_imagesContainer);
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
                token.ThrowIfCancellationRequested();
                await ((CloudBlob) blob).DeleteIfExistsAsync(token).ConfigureAwait(false);
            }
        }
    }
}
