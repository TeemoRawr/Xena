using Microsoft.Extensions.Options;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;

namespace Xena.Discovery;

internal class DiscoverBackgroundService : BackgroundService
{
    private readonly IXenaDiscoveryServicesService _xenaDiscoveryServicesService;
    private readonly IOptions<XenaDiscoveryOptions> _xenaDiscoveryOptions;

    public DiscoverBackgroundService(
        IXenaDiscoveryServicesService xenaDiscoveryServicesService,
        IOptions<XenaDiscoveryOptions> xenaDiscoveryOptions)
    {
        _xenaDiscoveryServicesService = xenaDiscoveryServicesService;
        _xenaDiscoveryOptions = xenaDiscoveryOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var xenaDiscoveryOptions = _xenaDiscoveryOptions.Value;

            try
            {
                await _xenaDiscoveryServicesService.RefreshServicesAsync(stoppingToken);
            }
            finally
            {
                await Task.Delay(xenaDiscoveryOptions.RefreshServicesTimeThreshold, stoppingToken);
            }
        }
    }
}