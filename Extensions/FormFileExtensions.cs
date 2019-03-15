namespace Clarity.Core
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Http;

    public static class FormFileExtensions
    {
        public static bool IsImage(this IFormFile file)
        {
            var imageExtensions = new[] { ".jpg", ".png", ".gif", ".jpeg" };
            return file.ContentType.Contains("image") ||
                   imageExtensions.Any(x => file.FileName.EndsWith(x, StringComparison.OrdinalIgnoreCase));
        }
    }
}