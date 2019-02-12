﻿namespace Cef.Core.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Http;

    [PublicAPI]
    public interface IStorageService
    {
        Task<Uri> UploadFileToStorageAsync(IFormFile file, string fileName);

        Task<Uri> UploadByteArrayToStorageAsync(byte[] buffer, string fileName);

        string GetSharedAccessSignature(string fileName, string uri);

        Task DeleteAllFromStorageAsync();
    }
}