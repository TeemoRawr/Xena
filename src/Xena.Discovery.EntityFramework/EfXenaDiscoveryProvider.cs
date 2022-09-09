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

    public async Task AddServiceAsync(Service service)
    {
        _context.Services.Add(service);
        await _context.SaveChangesAsync();
    }

    public async Task<Service?> GetServiceAsync(string id)
    {
        var service = await _context.Services.SingleOrDefaultAsync(s => s.Id == id);
        return service;
    }

    public async Task<IReadOnlyList<Service>> FindByTagAsync(string tag)
    {
        var services = await _context.Services.Where(p => p.Tags.Contains(tag)).ToListAsync();

        return services;
    }

    public Task RefreshServicesAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}