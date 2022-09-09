using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xena.Discovery.Configuration;
using Xena.Discovery.Interfaces;

namespace Xena.Discovery.Consul;

public static class ConsulXenaDiscoveryServicesExtensions
{
    public static IXenaDiscoveryServicesConfigurator AddConsulProvider(this IXenaDiscoveryServicesConfigurator xenaDiscoveryServicesConfigurator)
    {
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<ConsulXenaDiscoveryProvider>();
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<IXenaDiscoveryProvider>(p => p.GetRequiredService<ConsulXenaDiscoveryProvider>());

        xenaDiscoveryServicesConfigurator.AddPostBuildAction(application =>
        {
            var hostApplicationLifetime = application.Services.GetRequiredService<IHostApplicationLifetime>();
            var consulService = application.Services.GetRequiredService<ConsulXenaDiscoveryProvider>();

            hostApplicationLifetime.ApplicationStarted.Register(async () =>
            {
                await consulService.InitializeConsulAsync();
            });

            hostApplicationLifetime.ApplicationStopping.Register(async () =>
            {
                await consulService.DeactivateAsync();
            });
        });

        return xenaDiscoveryServicesConfigurator;
    }
}