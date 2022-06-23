using System.Diagnostics.CodeAnalysis;
using Xena.Discovery.Configuration;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;

namespace Xena.Discovery.Memory;

[ExcludeFromCodeCoverage]
public static class MemoryDiscoveryServicesExtensions
{
    public static IXenaDiscoveryServicesConfigurator AddMemoryDiscover(this IXenaDiscoveryServicesConfigurator xenaDiscoveryServicesConfigurator, IEnumerable<Service> services)
    {
        var memoryDiscoveryServicesService = new MemoryXenaDiscoveryServicesService(services);

        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton(_ => memoryDiscoveryServicesService);
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<IXenaInitializeDiscoveryServicesService>(p => p.GetRequiredService<MemoryXenaDiscoveryServicesService>());
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<IXenaDiscoveryServicesService>(p => p.GetRequiredService<MemoryXenaDiscoveryServicesService>());

        return xenaDiscoveryServicesConfigurator;
    }
}