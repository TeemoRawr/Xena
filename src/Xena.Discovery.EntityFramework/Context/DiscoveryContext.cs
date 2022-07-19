using Microsoft.EntityFrameworkCore;
using Xena.Discovery.Models;

namespace Xena.Discovery.EntityFramework.Context;

public class DiscoveryContext : DbContext
{
    public DiscoveryContext(DbContextOptions<DiscoveryContext> options) : base(options)
    {
    }

    public DbSet<Service> Services { get; private set; } = null!;
}