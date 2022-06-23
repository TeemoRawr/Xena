namespace Xena.Discovery.Interfaces;

public interface IXenaInitializeDiscoveryServicesService
{
    Task InitializeAsync(CancellationToken stoppingToken);
    bool Initialized { get; }
    Task DeactivateAsync();
}