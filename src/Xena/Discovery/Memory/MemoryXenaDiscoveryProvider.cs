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

    public Task AddServiceAsync(Service service)
    {
        _services.Add(service);
        return Task.CompletedTask;
    }

    public Task AddServiceTagAsync(params string[] tags)
    {
        return Task.CompletedTask;
    }

    public Service? GetService(string id)
    {
        var service = _services.FirstOrDefault(s => s.Id == id);
        return service;
    }

    public Task<Service?> GetServiceAsync(string id)
    {
        var service = _services.FirstOrDefault(s => s.Id == id);
        return Task.FromResult(service);
    }

    public Task<IReadOnlyList<Service>> FindByTagAsync(string tag)
    {
        var services = _services.Where(s => s.Tags.Contains(tag)).ToList();
        return Task.FromResult<IReadOnlyList<Service>>(services);
    }

    public Task RefreshServicesAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}