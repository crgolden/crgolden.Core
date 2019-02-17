namespace Clarity.Core
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public interface IStorageService
    {
        Task<Uri> UploadFileToStorageAsync(IFormFile file, string fileName);

        Task<Uri> UploadByteArrayToStorageAsync(byte[] buffer, string fileName);

        string GetSharedAccessSignature(string fileName, string uri);

        Task DeleteAllFromStorageAsync();
    }
}