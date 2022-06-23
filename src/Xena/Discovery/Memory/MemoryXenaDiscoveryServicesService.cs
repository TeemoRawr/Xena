using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;

namespace Xena.Discovery.Memory;

internal class MemoryXenaDiscoveryServicesService : IXenaDiscoveryServicesService, IXenaInitializeDiscoveryServicesService
{
    private readonly List<Service> _services;

    public MemoryXenaDiscoveryServicesService(IEnumerable<Service> services)
    {
        _services = services.ToList();
    }

    public Task AddServiceAsync(Service service)
    {
        _services.Add(service);
        return Task.CompletedTask;
    }

    public Task<Service?> GetServiceAsync(string id)
    {
        var service = _services.FirstOrDefault(s => s.Id == id);
        return Task.FromResult(service);
    }

    public Task RefreshServicesAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    public Task InitializeAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    public bool Initialized => _services.Any();

    public Task DeactivateAsync()
    {
        return Task.CompletedTask;
    }
}