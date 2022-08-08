using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Xena.Discovery.Configuration;
using Xena.Discovery.Models;
using Xena.Startup;

namespace Xena.Discovery;

[ExcludeFromCodeCoverage]
public static class DiscoveryServicesExtensions
{
    public static IXenaWebApplicationBuilder AddDiscoveryServicesService(
        this IXenaWebApplicationBuilder webApplicationBuilder, 
        Action<IXenaDiscoveryServicesConfigurator> configuratorAction)
    {
        var defaultXenaDiscoveryOptions = new XenaDiscoveryOptions();

        return AddDiscoveryServicesService(webApplicationBuilder, defaultXenaDiscoveryOptions, configuratorAction);
    }

    public static IXenaWebApplicationBuilder AddDiscoveryServicesService(
        this IXenaWebApplicationBuilder webApplicationBuilder, 
        XenaDiscoveryOptions options, 
        Action<IXenaDiscoveryServicesConfigurator> configuratorAction)
    {
        webApplicationBuilder.WebApplicationBuilder.Services.AddHostedService<XenaDiscoverBackgroundService>();

        var wrappedOptions = Options.Create(options);
        webApplicationBuilder.WebApplicationBuilder.Services.AddSingleton(_ => wrappedOptions);

        var configurator = new XenaDiscoveryServicesConfigurator(webApplicationBuilder);

        configuratorAction(configurator);

        return webApplicationBuilder;
    }
}