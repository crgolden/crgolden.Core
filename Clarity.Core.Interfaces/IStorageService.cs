namespace Clarity.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public interface IStorageService
    {
        Task<Uri> UploadFileToStorageAsync(
            IFormFile file,
            string fileName,
            CancellationToken token);

        Task<Uri> UploadByteArrayToStorageAsync(
            byte[] buffer,
            string fileName,
            CancellationToken token);

        string GetSharedAccessSignature(
            string fileName,
            string containerName);

        Task<bool> DeleteFileFromStorageAsync(CancellationToken token, Uri blobUri);

        Task DeleteAllFromStorageAsync(CancellationToken token, string containerName);
    }
}