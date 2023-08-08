using Xena.Discovery.Models;

namespace Xena.Discovery.Interfaces;

public interface IXenaDiscoveryProvider
{
    Task AddServiceAsync(Service service);
    Service? GetService(string id);
    Task<Service?> GetServiceAsync(string id);
    Task<IReadOnlyList<Service>> FindByTagAsync(string tag);
    Task RefreshServicesAsync(CancellationToken stoppingToken);
}