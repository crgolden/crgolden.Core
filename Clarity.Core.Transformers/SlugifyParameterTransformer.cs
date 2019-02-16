namespace Clarity.Core
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Routing;

    [PublicAPI]
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
