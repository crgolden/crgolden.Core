namespace Cef.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Http;

    [PublicAPI]
    public interface IStorageService
    {
        Task<Uri> UploadFileToStorageAsync(IFormFile file, string fileName, string containerName);

        Task<Uri> UploadByteArrayToStorageAsync(byte[] buffer, string fileName, string containerName);

        string GetSharedAccessSignature(string fileName, string containerName);

        Task DeleteAllFromStorageAsync(IEnumerable<string> containerNames);
    }
}