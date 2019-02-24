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
            CancellationToken cancellationToken);

        Task<Uri> UploadByteArrayToStorageAsync(
            byte[] buffer,
            string fileName,
            CancellationToken cancellationToken);

        string GetSharedAccessSignature(
            string fileName,
            string containerName);

        Task DeleteAllFromStorageAsync(CancellationToken cancellationToken);
    }
}