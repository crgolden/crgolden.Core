namespace crgolden.Core
{
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class SwaggerGenConfiguration : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
        private readonly string _defaultScheme;
        private readonly ApiKeyScheme _apiKeyScheme;
        private readonly Info _info;

        public SwaggerGenConfiguration(
            IApiVersionDescriptionProvider apiVersionDescriptionProvider,
            IOptions<Shared.ApiExplorerOptions> apiExplorerOptions)
        {
            _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
            _defaultScheme = apiExplorerOptions.Value?.DefaultScheme;
            _apiKeyScheme = new ApiKeyScheme
            {
                Name = apiExplorerOptions.Value?.ApiKeySchemeName,
                Description = apiExplorerOptions.Value?.ApiKeySchemeDescription,
                In = apiExplorerOptions.Value?.ApiKeySchemeIn,
                Type = apiExplorerOptions.Value?.ApiKeySchemeType
            };
            _info = new Info
            {
                Title = apiExplorerOptions.Value?.Title,
                Description = apiExplorerOptions.Value?.Description,
                TermsOfService = apiExplorerOptions.Value?.TermsOfService,
                Contact = new Contact
                {
                    Name = apiExplorerOptions.Value?.ContactName,
                    Email = apiExplorerOptions.Value?.ContactEmail,
                    Url = apiExplorerOptions.Value?.ContactUrl
                },
                License = new License
                {
                    Name = apiExplorerOptions.Value?.LicenseName,
                    Url = apiExplorerOptions.Value?.LicenseUrl
                }
            };
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                _info.Version = $"{description.ApiVersion}";
                if (description.IsDeprecated) _info.Description += " This API version has been deprecated.";
                options.SwaggerDoc(description.GroupName, _info);
                options.AddSecurityDefinition(_defaultScheme, _apiKeyScheme);
            }
        }
    }
}
