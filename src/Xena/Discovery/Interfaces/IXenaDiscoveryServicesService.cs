using Xena.Discovery.Models;

namespace Xena.Discovery.Interfaces;

public interface IXenaDiscoveryServicesService
{
    Task AddServiceAsync(Service service);
    Task<Service?> GetServiceAsync(string id);
    Task RefreshServicesAsync(CancellationToken stoppingToken);
}