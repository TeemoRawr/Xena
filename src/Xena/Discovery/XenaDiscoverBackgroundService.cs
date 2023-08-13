using Microsoft.Extensions.Options;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;

namespace Xena.Discovery;

internal class XenaDiscoverBackgroundService : BackgroundService
{
    private readonly IXenaDiscoveryProvider _xenaDiscoveryProvider;
    private readonly IXenaDiscoveryFinalizerService _xenaDiscoveryFinalizerService;
    private readonly IOptions<XenaDiscoveryOptions> _xenaDiscoveryOptions;

    public XenaDiscoverBackgroundService(
        IXenaDiscoveryProvider xenaDiscoveryProvider,
        IXenaDiscoveryFinalizerService xenaDiscoveryFinalizerService,
        IOptions<XenaDiscoveryOptions> xenaDiscoveryOptions)
    {
        _xenaDiscoveryProvider = xenaDiscoveryProvider;
        _xenaDiscoveryFinalizerService = xenaDiscoveryFinalizerService;

        _xenaDiscoveryOptions = xenaDiscoveryOptions;
    }



    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var xenaDiscoveryOptions = _xenaDiscoveryOptions.Value;

            try
            {
                await _xenaDiscoveryProvider.RefreshServicesAsync(stoppingToken);
            }
            finally
            {
                await Task.Delay(xenaDiscoveryOptions.RefreshServicesTimeThreshold, stoppingToken);
            }
        }

        await _xenaDiscoveryFinalizerService.FinalizeAsync();
    }
}