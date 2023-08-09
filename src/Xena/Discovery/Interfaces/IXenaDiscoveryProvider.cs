using Xena.Discovery.Models;

namespace Xena.Discovery.Interfaces;

public interface IXenaDiscoveryProvider
{
    Service? GetService(string id);
    Task RefreshServicesAsync(CancellationToken stoppingToken);
}