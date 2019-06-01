namespace crgolden.Core
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Routing;

    [ExcludeFromCodeCoverage]
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
