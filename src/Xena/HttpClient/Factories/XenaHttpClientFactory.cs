﻿using System.Reflection;
using Refit;
using Xena.Discovery.Interfaces;
using Xena.HttpClient.Interceptors;
using Xena.HttpClient.Interceptors.Interfaces;
using Xena.HttpClient.Models;

namespace Xena.HttpClient.Factories;

internal class XenaHttpClientFactory
{
    private readonly IXenaDiscoveryProvider _discoveryProvider;
    private readonly IXenaHttpClientInterceptor[] _xenaHttpClientHandlers;
    private readonly ILogger<XenaHttpClientFactory> _logger;

    public XenaHttpClientFactory(
        IXenaDiscoveryProvider? discoveryServicesService,
        IEnumerable<IXenaHttpClientInterceptor> xenaHttpClientHandlers,
        ILogger<XenaHttpClientFactory> logger)
    {
        _discoveryProvider = discoveryServicesService ?? 
                                    throw new NullReferenceException(
                                        $"Interface {nameof(IXenaDiscoveryProvider)} is not registered. " +
                                                                    "Please add Discovery module to application");
        _logger = logger;
        _xenaHttpClientHandlers = xenaHttpClientHandlers.ToArray();
    }

    public THttpClient CreateHttpClient<THttpClient>(
        Func<HttpRequestMessage, Task<string>>? authorizationHeaderFunc = null) 
        where THttpClient : IXenaHttpClient
    {
        var httpClientType = typeof(THttpClient);
        var httpClientNameAttribute = httpClientType.GetCustomAttribute<XenaHttpClientAttribute>();

        if (httpClientNameAttribute is null || string.IsNullOrWhiteSpace(httpClientNameAttribute.Name))
        {
            throw new InvalidOperationException("You need to set XenaHttpClientAttribute with name of service on interface");
        }

        var serviceId = httpClientNameAttribute.Name;

        var service = _discoveryProvider.GetService(serviceId);

        if (service is null)
        {
            var noServiceException = new Exception($"Not found registered service for HttpClient {serviceId}");

            _logger.LogError(noServiceException, $"{nameof(XenaHttpClientFactory)}: Error occurred while creating HttpClient");
            throw noServiceException;
        }

        var serviceUrl = $"{service.Address}:{ service.Port}";
        _logger.LogDebug($"Address for HttpClient interface {serviceId}: {serviceUrl}");

        var xenaHttpClient = RestService.For<THttpClient>(serviceUrl, new RefitSettings
        {
            AuthorizationHeaderValueWithParamGetter = authorizationHeaderFunc,
            HttpMessageHandlerFactory = () => new XenaHttpClientHandler(_xenaHttpClientHandlers),
        });

        return xenaHttpClient;
    }
}