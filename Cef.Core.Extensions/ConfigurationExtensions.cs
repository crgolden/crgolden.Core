namespace Cef.Core.Extensions
{
    using JetBrains.Annotations;
    using Microsoft.Extensions.Configuration;

    [PublicAPI]
    public static class ConfigurationExtensions
    {
        public static T GetOptions<T>(this IConfiguration configuration)
        {
            var optionsSection = configuration.GetSection(nameof(T));
            return optionsSection.Exists() ? optionsSection.Get<T>() : default(T);
        }
    }
}
