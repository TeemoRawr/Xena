using Microsoft.Extensions.DependencyInjection;
using Xena.Discovery.Interfaces;
using Xena.HttpClient.Factories;
using Xena.HttpClient.Models;
using Xena.Startup;

namespace Xena.HttpClient.Configuration;

internal class XenaHttpClientConfigurator : IXenaHttpClientConfigurator
{
    private readonly IXenaWebApplicationBuilder _xenaWebApplicationBuilder;

    public XenaHttpClientConfigurator(IXenaWebApplicationBuilder xenaWebApplicationBuilder)
    {
        _xenaWebApplicationBuilder = xenaWebApplicationBuilder;
    }

    public IXenaHttpClientConfigurator AddHttpClient<THttpClient>() where THttpClient : IXenaHttpClient
    {
        _xenaWebApplicationBuilder.WebApplicationBuilder.Services.AddScoped(provider =>
        {
            var xenaHttpClientFactory = provider.GetRequiredService<XenaHttpClientFactory>();

            return xenaHttpClientFactory.CreateHttpClient<THttpClient>();
        });

        _xenaWebApplicationBuilder.AddPostBuildAction(application =>
        {
            var httpClientName = typeof(THttpClient).FullName;

            var discoveryServicesService = application.Services.GetRequiredService<IXenaDiscoveryServicesService>();

            discoveryServicesService.AddServiceTagAsync(httpClientName!);
        });

        return this;
    }
}