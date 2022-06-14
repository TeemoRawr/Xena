using System.Diagnostics.CodeAnalysis;
using ESF.Discovery.Configuration;
using ESF.Startup;

namespace ESF.Discovery;

[ExcludeFromCodeCoverage]
public static class DiscoveryServicesExtensions
{
    public static IEsfWebApplicationBuilder AddDiscoveryServicesService(this IEsfWebApplicationBuilder webApplicationBuilder, Action<IDiscoveryServicesConfigurator> configuratorAction)
    {
        webApplicationBuilder.WebApplicationBuilder.Services.AddHostedService<DiscoverBackgroundService>();
        var configurator = new DiscoveryServicesConfigurator(webApplicationBuilder);

        configuratorAction(configurator);

        return webApplicationBuilder;
    }
}