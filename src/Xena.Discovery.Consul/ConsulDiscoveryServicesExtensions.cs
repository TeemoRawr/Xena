using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xena.Discovery.Configuration;
using Xena.Discovery.Interfaces;

namespace Xena.Discovery.Consul;

[ExcludeFromCodeCoverage]
public static class ConsulDiscoveryServicesExtensions
{
    public static IXenaDiscoveryServicesConfigurator AddConsulDiscover(this IXenaDiscoveryServicesConfigurator xenaDiscoveryServicesConfigurator)
    {
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<ConsulXenaDiscoveryServicesService>();
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<IXenaDiscoveryServicesService>(p => p.GetRequiredService<ConsulXenaDiscoveryServicesService>());

        xenaDiscoveryServicesConfigurator.AddPostBuildAction(application =>
        {
            var hostApplicationLifetime = application.Services.GetRequiredService<IHostApplicationLifetime>();
            var consulService = application.Services.GetRequiredService<ConsulXenaDiscoveryServicesService>();

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