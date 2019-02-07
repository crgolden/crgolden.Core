namespace Cef.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Http;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;

    [PublicAPI]
    public static class AzureFilesUtility
    {
        public static bool IsImage(IFormFile file)
        {
            var imageExtensions = new[] {".jpg", ".png", ".gif", ".jpeg"};
            return file.ContentType.Contains("image") ||
                   imageExtensions.Any(x => file.FileName.EndsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        public static async Task<Uri> UploadFileToStorageAsync(
            IFormFile file,
            string fileName,
            string accountName,
            string accountKey,
            string containerName)
        {
            using (var stream = file.OpenReadStream())
            {
                var storageCredentials = new StorageCredentials(
                    accountName: accountName,
                    keyValue: accountKey);
                var storageAccount = new CloudStorageAccount(storageCredentials, true);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(containerName);
                var blockBlob = container.GetBlockBlobReference(fileName);
                await blockBlob.UploadFromStreamAsync(stream);
                return blockBlob.Uri;
            }
        }

        public static async Task<Uri> UploadByteArrayToStorageAsync(
            byte[] buffer,
            string fileName,
            string accountName,
            string accountKey,
            string containerName)
        {
            var storageCredentials = new StorageCredentials(
                accountName: accountName,
                keyValue: accountKey);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            var blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromByteArrayAsync(buffer, 0, buffer.Length);
            return blockBlob.Uri;
        }

        public static string GetSharedAccessSignature(
            string accountName,
            string accountKey,
            string containerName,
            string fileName)
        {
            var storageCredentials = new StorageCredentials(
                accountName: accountName,
                keyValue: accountKey);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);
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

        public static async Task DeleteAllFromStorageAsync(
            string accountName,
            string accountKey,
            IEnumerable<string> containerNames)
        {
            var storageCredentials = new StorageCredentials(
                accountName: accountName,
                keyValue: accountKey);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
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
