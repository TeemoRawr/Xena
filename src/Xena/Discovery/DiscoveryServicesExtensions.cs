using System.Diagnostics.CodeAnalysis;
using Xena.Discovery.Configuration;
using Xena.Startup;

namespace Xena.Discovery;

[ExcludeFromCodeCoverage]
public static class DiscoveryServicesExtensions
{
    public static IXenaWebApplicationBuilder AddDiscoveryServicesService(this IXenaWebApplicationBuilder webApplicationBuilder, Action<IDiscoveryServicesConfigurator> configuratorAction)
    {
        webApplicationBuilder.WebApplicationBuilder.Services.AddHostedService<DiscoverBackgroundService>();
        var configurator = new DiscoveryServicesConfigurator(webApplicationBuilder);

        configuratorAction(configurator);

        return webApplicationBuilder;
    }
}