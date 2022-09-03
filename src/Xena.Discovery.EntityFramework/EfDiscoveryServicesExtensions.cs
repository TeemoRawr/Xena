using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xena.Discovery.Configuration;
using Xena.Discovery.EntityFramework.Context;
using Xena.Discovery.Interfaces;

namespace Xena.Discovery.EntityFramework;

[ExcludeFromCodeCoverage]
public static class EfDiscoveryServicesExtensions
{
    public static IXenaDiscoveryServicesConfigurator AddEfProvider(
        this IXenaDiscoveryServicesConfigurator xenaDiscoveryServicesConfigurator, 
        Action<DbContextOptionsBuilder<DiscoveryContext>>? contextOptionsBuilderAction = null)
    {
        var contextOptionsBuilder = new DbContextOptionsBuilder<DiscoveryContext>();

        contextOptionsBuilderAction?.Invoke(contextOptionsBuilder);

        var dbContextOptions = contextOptionsBuilder.Options;

        xenaDiscoveryServicesConfigurator.ServiceCollection.AddTransient(_ => dbContextOptions);
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddTransient<DiscoveryContext>();

        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<EfXenaDiscoveryServicesService>();
        xenaDiscoveryServicesConfigurator.ServiceCollection.AddSingleton<IXenaDiscoveryServicesService>(p => p.GetRequiredService<EfXenaDiscoveryServicesService>());
        
        return xenaDiscoveryServicesConfigurator;
    }
}