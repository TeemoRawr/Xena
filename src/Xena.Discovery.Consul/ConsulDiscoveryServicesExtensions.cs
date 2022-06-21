using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xena.Discovery.Configuration;
using Xena.Discovery.Interfaces;

namespace Xena.Discovery.Consul;

public static class ConsulDiscoveryServicesExtensions
{
    public static IDiscoveryServicesConfigurator AddConsulDiscover(this IDiscoveryServicesConfigurator discoveryServicesConfigurator)
    {
        discoveryServicesConfigurator.ServiceCollection.AddSingleton<ConsulDiscoveryServicesService>();
        discoveryServicesConfigurator.ServiceCollection.AddSingleton<IInitializeDiscoveryServicesService>(p => p.GetRequiredService<ConsulDiscoveryServicesService>());
        discoveryServicesConfigurator.ServiceCollection.AddSingleton<IDiscoveryServicesService>(p => p.GetRequiredService<ConsulDiscoveryServicesService>());

        discoveryServicesConfigurator.AddPostBuildAction(application =>
        {
            var hostApplicationLifetime = application.Services.GetRequiredService<IHostApplicationLifetime>();
            var consulService = application.Services.GetRequiredService<ConsulDiscoveryServicesService>();

            hostApplicationLifetime.ApplicationStarted.Register(async () =>
            {
                await consulService.InitializeConsulAsync();
            });
        });

        return discoveryServicesConfigurator;
    }
}