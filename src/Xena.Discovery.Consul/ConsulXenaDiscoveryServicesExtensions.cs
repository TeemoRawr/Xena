using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Xena.Discovery.Configuration;
using Xena.Discovery.Consul.Configuration;
using Xena.Discovery.Interfaces;

namespace Xena.Discovery.Consul;

public static class ConsulXenaDiscoveryServicesExtensions
{
    public static IXenaDiscoveryServicesConfigurator AddConsulProvider(this IXenaDiscoveryServicesConfigurator xenaDiscoveryServicesConfigurator)
    {
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<IConsulXenaDiscoveryProvider, ConsulXenaDiscoveryProvider>();
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<IXenaDiscoveryProvider>(p => p.GetRequiredService<IConsulXenaDiscoveryProvider>());
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<IConsulClient>(provider =>
        {
            var consulOptions = provider.GetRequiredService<IOptions<ConsulXenaDiscoveryServicesConfiguration>>();

            var consulDiscoveryServicesConfiguration = consulOptions.Value;

            if (consulDiscoveryServicesConfiguration is null)
            {
                throw new ArgumentNullException(
                    nameof(consulDiscoveryServicesConfiguration),
                    "Missing configuration for Consul provider");
            }

            var consulClient = new ConsulClient(configuration =>
            {
                configuration.Address = new Uri(consulDiscoveryServicesConfiguration.Host);
            });

            return consulClient;
        });

        xenaDiscoveryServicesConfigurator.AddPostBuildAction(application =>
        {
            var hostApplicationLifetime = application.Services.GetRequiredService<IHostApplicationLifetime>();
            var consulService = application.Services.GetRequiredService<IConsulXenaDiscoveryProvider>();

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