using Xena.Discovery.Configuration;
using Xena.Startup.Interfaces;

namespace Xena.Discovery;

public static class XenaDiscoveryServicesExtensions
{
    /// <summary>
    /// Adds required services for Discovery feature
    /// </summary>
    /// <param name="webApplicationBuilder">
    /// The <see cref="IXenaWebApplicationBuilder"/> to add the services to.
    /// </param>
    /// <param name="configurationAction">
    /// Action to configure Discovery feature
    /// </param>
    /// <returns>
    /// The <see cref="IXenaWebApplicationBuilder"/> so that additional calls can be chained
    /// </returns>
    public static IXenaWebApplicationBuilder AddDiscovery(
        this IXenaWebApplicationBuilder webApplicationBuilder, 
        Action<IXenaDiscoveryServicesConfigurator> configurationAction)
    {
        webApplicationBuilder.Services.AddHostedService<XenaDiscoverBackgroundService>();

        var configurator = new XenaDiscoveryServicesConfigurator(webApplicationBuilder);

        configurationAction(configurator);

        return webApplicationBuilder;
    }
}