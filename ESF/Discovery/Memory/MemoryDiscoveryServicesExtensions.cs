using ESF.Discovery.Configuration;
using ESF.Discovery.Interfaces;
using ESF.Discovery.Models;

namespace ESF.Discovery.Memory;

public static class MemoryDiscoveryServicesExtensions
{
    public static IDiscoveryServicesConfigurator AddMemoryDiscover(this IDiscoveryServicesConfigurator discoveryServicesConfigurator, IEnumerable<Service> services)
    {
        var memoryDiscoveryServicesService = new MemoryDiscoveryServicesService(services);

        discoveryServicesConfigurator.ServiceCollection.AddSingleton(_ => memoryDiscoveryServicesService);
        discoveryServicesConfigurator.ServiceCollection.AddSingleton<IInitializeDiscoveryServicesService>(p => p.GetRequiredService<MemoryDiscoveryServicesService>());
        discoveryServicesConfigurator.ServiceCollection.AddSingleton<IDiscoveryServicesService>(p => p.GetRequiredService<MemoryDiscoveryServicesService>());

        return discoveryServicesConfigurator;
    }
}