using System.Diagnostics.CodeAnalysis;
using Xena.Discovery.Configuration;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;

namespace Xena.Discovery.Memory;

[ExcludeFromCodeCoverage]
public static class MemoryDiscoveryServicesExtensions
{
    public static IXenaDiscoveryServicesConfigurator AddMemoryProvider(this IXenaDiscoveryServicesConfigurator xenaDiscoveryServicesConfigurator, IEnumerable<Service> services)
    {
        var memoryDiscoveryServicesService = new MemoryXenaDiscoveryProvider(services);

        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton(_ => memoryDiscoveryServicesService);
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<IXenaDiscoveryProvider>(p => p.GetRequiredService<MemoryXenaDiscoveryProvider>());

        return xenaDiscoveryServicesConfigurator;
    }
}