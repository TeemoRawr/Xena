using Xena.MemoryBus.Configurator;
using Xena.MemoryBus.Interfaces;
using Xena.Startup.Interfaces;

namespace Xena.MemoryBus;

public static class XenaMemoryBusServicesExtensions
{
    /// <summary>
    /// Adds required services for Memory Bus feature
    /// </summary>
    /// <param name="webApplicationBuilder">
    /// The <see cref="IXenaWebApplicationBuilder"/> to add the services to.
    /// </param>
    /// <param name="configurationAction">
    /// Action to configure Memory Bus feature
    /// </param>
    /// <returns>
    /// The <see cref="IXenaWebApplicationBuilder"/> so that additional calls can be chained
    /// </returns>
    public static IXenaWebApplicationBuilder AddMemoryBus(
        this IXenaWebApplicationBuilder builder,
        Action<IXenaMemoryBusConfigurator>? configurationAction = null)
    {
        var configurator = new XenaMemoryBusConfigurator(builder);
        configurationAction?.Invoke(configurator);
        
        builder.Services.AddTransient<XenaCommandBus>();
        builder.Services.AddTransient<XenaQueryBus>();
        builder.Services.AddTransient<XenaEventBus>();
        builder.Services.AddTransient<IXenaMemoryBus, XenaMemoryBus>();

        return builder;
    }
}