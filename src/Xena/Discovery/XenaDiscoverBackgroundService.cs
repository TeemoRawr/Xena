using Microsoft.Extensions.Options;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;

namespace Xena.Discovery;

internal class XenaDiscoverBackgroundService : BackgroundService
{
    private readonly IXenaDiscoveryServicesProvider _xenaDiscoveryServicesProvider;
    private readonly IOptions<XenaDiscoveryOptions> _xenaDiscoveryOptions;

    public XenaDiscoverBackgroundService(
        IXenaDiscoveryServicesProvider xenaDiscoveryServicesProvider,
        IOptions<XenaDiscoveryOptions> xenaDiscoveryOptions)
    {
        _xenaDiscoveryServicesProvider = xenaDiscoveryServicesProvider;
        _xenaDiscoveryOptions = xenaDiscoveryOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var xenaDiscoveryOptions = _xenaDiscoveryOptions.Value;

            try
            {
                await _xenaDiscoveryServicesProvider.RefreshServicesAsync(stoppingToken);
            }
            finally
            {
                await Task.Delay(xenaDiscoveryOptions.RefreshServicesTimeThreshold, stoppingToken);
            }
        }
    }
}