using Xena.Discovery.Configuration;
using Xena.Startup.Interfaces;

namespace Xena.Discovery;

public static class XenaDiscoveryServicesExtensions
{
    /// <summary>
    /// Add required services for discovery feature
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    /// <param name="configuratorAction">
    /// Action which allow to configure discovery service
    /// </param>
    /// <returns></returns>
    public static IXenaWebApplicationBuilder AddDiscovery(
        this IXenaWebApplicationBuilder webApplicationBuilder, 
        Action<IXenaDiscoveryServicesConfigurator> configuratorAction)
    {
        webApplicationBuilder.Services.AddHostedService<XenaDiscoverBackgroundService>();
        webApplicationBuilder.Services.AddTransient<IStartupFilter, XenaDiscoveryStartupFilter>();

        var configurator = new XenaDiscoveryServicesConfigurator(webApplicationBuilder);

        configuratorAction(configurator);

        return webApplicationBuilder;
    }
}