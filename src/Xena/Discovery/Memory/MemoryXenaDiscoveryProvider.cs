using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;

namespace Xena.Discovery.Memory;

internal class MemoryXenaDiscoveryProvider : IXenaDiscoveryProvider
{
    private readonly List<Service> _services;

    public MemoryXenaDiscoveryProvider(IEnumerable<Service> services)
    {
        _services = services.ToList();
    }

    public Service? GetService(string id)
    {
        var service = _services.FirstOrDefault(s => s.Id == id);
        return service;
    }

    public Task RefreshServicesAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}