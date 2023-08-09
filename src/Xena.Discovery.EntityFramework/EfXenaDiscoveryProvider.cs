using Microsoft.EntityFrameworkCore;
using Xena.Discovery.EntityFramework.Context;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;

namespace Xena.Discovery.EntityFramework;

internal class EfXenaDiscoveryProvider : IXenaDiscoveryProvider
{
    private readonly DiscoveryContext _context;

    public EfXenaDiscoveryProvider(DiscoveryContext context)
    {
        _context = context;
    }

    public Service? GetService(string id)
    {
        var service = _context.Services.SingleOrDefault(s => s.Id == id);
        return service;
    }

    public async Task<Service?> GetServiceAsync(string id)
    {
        var service = await _context.Services.SingleOrDefaultAsync(s => s.Id == id);
        return service;
    }

    public Task RefreshServicesAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }    
}