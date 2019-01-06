﻿namespace Cef.Core.Transformers
{
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Routing;

    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            return value == null
                ? null
                // Slugify value
                : Regex.Replace($"{value}", "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
