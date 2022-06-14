namespace ESF.Discovery.Interfaces;

public interface IInitializeDiscoveryServicesService
{
    Task InitializeAsync(CancellationToken stoppingToken);
    bool Initialized { get; }
    Task DeactivateAsync();
}