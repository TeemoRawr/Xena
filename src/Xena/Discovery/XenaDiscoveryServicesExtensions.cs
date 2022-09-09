using Xena.Discovery.Configuration;
using Xena.Startup.Interfaces;

namespace Xena.Discovery;

public static class XenaDiscoveryServicesExtensions
{
    public static IXenaWebApplicationBuilder AddDiscovery(
        this IXenaWebApplicationBuilder webApplicationBuilder, 
        Action<IXenaDiscoveryServicesConfigurator> configuratorAction)
    {
        webApplicationBuilder.Services.AddHostedService<XenaDiscoverBackgroundService>();

        var configurator = new XenaDiscoveryServicesConfigurator(webApplicationBuilder);

        configuratorAction(configurator);

        return webApplicationBuilder;
    }
}