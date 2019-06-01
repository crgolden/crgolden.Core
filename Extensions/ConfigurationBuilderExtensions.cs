namespace crgolden.Core
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.Services.AppAuthentication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.AzureKeyVault;

    [ExcludeFromCodeCoverage]
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAzureKeyVault(this IConfigurationBuilder configBuilder)
        {
            var configRoot = configBuilder.Build();
            var keyVaultName = configRoot.GetValue<string>("KeyVaultName");
            if (string.IsNullOrEmpty(keyVaultName)) { return configBuilder; }

            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(
                authenticationCallback: new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback));
            configBuilder.AddAzureKeyVault(
                vault: $"https://{keyVaultName}.vault.azure.net/",
                client: keyVaultClient,
                manager: new DefaultKeyVaultSecretManager());
            return configBuilder;
        }
    }
}
