using ESF.Discovery.Interfaces;

namespace ESF.Discovery;

internal class DiscoverBackgroundService : BackgroundService
{
    private readonly IInitializeDiscoveryServicesService _initializeDiscoveryServicesService;
    private readonly IDiscoveryServicesService _discoveryServicesService;

    public DiscoverBackgroundService(
        IInitializeDiscoveryServicesService initializeDiscoveryServicesService, 
        IDiscoveryServicesService discoveryServicesService)
    {
        _initializeDiscoveryServicesService = initializeDiscoveryServicesService;
        _discoveryServicesService = discoveryServicesService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try 
        {
            await _initializeDiscoveryServicesService.InitializeAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _discoveryServicesService.RefreshServicesAsync(stoppingToken);
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
        }
        finally
        {
            await _initializeDiscoveryServicesService.DeactivateAsync();
        }
    }
}