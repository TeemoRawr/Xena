using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Xena.Discovery.Configuration;
using Xena.Discovery.Models;
using Xena.Startup.Interfaces;

namespace Xena.Discovery;

[ExcludeFromCodeCoverage]
public static class XenaDiscoveryServicesExtensions
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
        webApplicationBuilder.Services.AddHostedService<XenaDiscoverBackgroundService>();

        var wrappedOptions = Options.Create(options);
        webApplicationBuilder.Services.AddSingleton(_ => wrappedOptions);

        var configurator = new XenaDiscoveryServicesConfigurator(webApplicationBuilder);

        configuratorAction(configurator);

        return webApplicationBuilder;
    }
}