using Xena.Discovery.Models;

namespace Xena.Discovery.Interfaces;

public interface IXenaDiscoveryServicesProvider
{
    Task AddServiceAsync(Service service);
    Task AddServiceTagAsync(params string[] tags);
    Task<Service?> GetServiceAsync(string id);
    Task<IReadOnlyList<Service>> FindByTagAsync(string tag);
    Task RefreshServicesAsync(CancellationToken stoppingToken);
}