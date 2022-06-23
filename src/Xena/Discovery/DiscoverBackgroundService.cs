using Microsoft.Extensions.Options;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;

namespace Xena.Discovery;

internal class DiscoverBackgroundService : BackgroundService
{
    private readonly IXenaInitializeDiscoveryServicesService _xenaInitializeDiscoveryServicesService;
    private readonly IXenaDiscoveryServicesService _xenaDiscoveryServicesService;
    private readonly IOptions<XenaDiscoveryOptions> _xenaDiscoveryOptions;

    public DiscoverBackgroundService(
        IXenaInitializeDiscoveryServicesService xenaInitializeDiscoveryServicesService, 
        IXenaDiscoveryServicesService xenaDiscoveryServicesService,
        IOptions<XenaDiscoveryOptions> xenaDiscoveryOptions)
    {
        _xenaInitializeDiscoveryServicesService = xenaInitializeDiscoveryServicesService;
        _xenaDiscoveryServicesService = xenaDiscoveryServicesService;
        _xenaDiscoveryOptions = xenaDiscoveryOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try 
        {
            await _xenaInitializeDiscoveryServicesService.InitializeAsync(stoppingToken);

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
        finally
        {
            await _xenaInitializeDiscoveryServicesService.DeactivateAsync();
        }
    }
}