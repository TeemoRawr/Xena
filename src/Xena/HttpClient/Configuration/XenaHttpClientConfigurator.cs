using Xena.HttpClient.Factories;
using Xena.HttpClient.Models;
using Xena.Startup.Interfaces;

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
        _xenaWebApplicationBuilder.Services.AddScoped(provider =>
        {
            var xenaHttpClientFactory = provider.GetRequiredService<XenaHttpClientFactory>();

            return xenaHttpClientFactory.CreateHttpClient<THttpClient>();
        });

        return this;
    }
}