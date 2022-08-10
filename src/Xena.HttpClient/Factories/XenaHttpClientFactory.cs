﻿using Microsoft.Extensions.Logging;
using Refit;
using Xena.Discovery.Interfaces;
using Xena.HttpClient.Models;

namespace Xena.HttpClient.Factories;

internal class XenaHttpClientFactory
{
    private readonly IXenaDiscoveryServicesService _discoveryServicesService;
    private readonly ILogger<XenaHttpClientFactory> _logger;

    public XenaHttpClientFactory(IXenaDiscoveryServicesService discoveryServicesService, ILogger<XenaHttpClientFactory> logger)
    {
        _discoveryServicesService = discoveryServicesService;
        _logger = logger;
    }

    public async Task<THttpClient> CreateHttpClient<THttpClient>() where THttpClient : IXenaHttpClient
    {
        var httpClientName = typeof(THttpClient).FullName;
        _logger.LogDebug($"Create HttpClient interface {httpClientName}");

        var services = await _discoveryServicesService.FindByTagAsync(httpClientName!);

        if (!services.Any())
        {
            var noServiceException = new Exception($"Not found registered service for HttpClient {httpClientName}");

            _logger.LogError(noServiceException, $"{nameof(XenaHttpClientFactory)}: Error occurred while creating HttpClient");
            throw noServiceException;
        }

        if (services.Count > 1)
        {
            var servicesAsString = string.Join(", ", services.Select(s => s.Id));

            var moreThenOneServiceException = new Exception($"Found more then one registered services for HttpClient {httpClientName}. Services: {servicesAsString}");
            _logger.LogError(moreThenOneServiceException, $"{nameof(XenaHttpClientFactory)}: Error occurred while creating HttpClient");

            throw moreThenOneServiceException;
        }

        var service = services.Single();

        var serviceUrl = $"{service.Address}:{ service.Port}";
        _logger.LogDebug($"Address for HttpClient interface {httpClientName}: {serviceUrl}");

        var xenaHttpClient = RestService.For<THttpClient>(serviceUrl);

        return xenaHttpClient;
    }
}