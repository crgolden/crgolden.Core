﻿namespace Clarity.Core
{
    using System;
    using Microsoft.AspNetCore.Http;

    public static class HttpRequestExtensions
    {
        public static string GetOrigin(this HttpRequest request)
        {
            string origin;

            if (request.Headers.TryGetValue("Referer", out var referrer))
            {
                var uri = new Uri(referrer);
                origin = $"{uri.GetLeftPart(UriPartial.Scheme)}{uri.Host}";
            }
            else
            {
                origin = $"{request.Scheme}://{request.Host}";
            }

            return origin;
        }
    }
}
