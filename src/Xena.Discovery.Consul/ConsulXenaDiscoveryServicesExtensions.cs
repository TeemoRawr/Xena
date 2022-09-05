using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xena.Discovery.Configuration;
using Xena.Discovery.Interfaces;

namespace Xena.Discovery.Consul;

[ExcludeFromCodeCoverage]
public static class ConsulXenaDiscoveryServicesExtensions
{
    public static IXenaDiscoveryServicesConfigurator AddConsulProvider(this IXenaDiscoveryServicesConfigurator xenaDiscoveryServicesConfigurator)
    {
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<ConsulXenaDiscoveryServicesProvider>();
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<IXenaDiscoveryServicesProvider>(p => p.GetRequiredService<ConsulXenaDiscoveryServicesProvider>());

        xenaDiscoveryServicesConfigurator.AddPostBuildAction(application =>
        {
            var hostApplicationLifetime = application.Services.GetRequiredService<IHostApplicationLifetime>();
            var consulService = application.Services.GetRequiredService<ConsulXenaDiscoveryServicesProvider>();

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