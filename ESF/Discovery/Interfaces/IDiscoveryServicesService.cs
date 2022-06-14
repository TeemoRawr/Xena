using ESF.Discovery.Models;

namespace ESF.Discovery.Interfaces;

public interface IDiscoveryServicesService
{
    Task AddServiceAsync(Service service);
    Task<Service?> GetServiceAsync(string id);
    Task RefreshServicesAsync(CancellationToken stoppingToken);
}