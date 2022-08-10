using Refit;
using Xena.Discovery.Interfaces;
using Xena.HttpClient.Models;

namespace Xena.HttpClient.Factories;

internal class XenaHttpClientFactory
{
    private readonly IXenaDiscoveryServicesService _discoveryServicesService;

    public XenaHttpClientFactory(IXenaDiscoveryServicesService discoveryServicesService)
    {
        _discoveryServicesService = discoveryServicesService;
    }

    public async Task<THttpClient> CreateHttpClient<THttpClient>() where THttpClient : IXenaHttpClient
    {
        var httpClientName = typeof(THttpClient).FullName;

        var services = await _discoveryServicesService.FindByTagAsync(httpClientName!);

        if (!services.Any())
        {
            throw new Exception($"Not found registered service for HttpClient {httpClientName}");
        }

        if (services.Count > 1)
        {
            var servicesAsString = string.Join(", ", services.Select(s => s.Id));

            throw new Exception($"Found more then one registered services for HttpClient {httpClientName}. Services: {servicesAsString}");
        }

        var service = services.Single();
        
        var xenaHttpClient = RestService.For<THttpClient>($"{service.Address}:{ service.Port}");

        return xenaHttpClient;
    }
}