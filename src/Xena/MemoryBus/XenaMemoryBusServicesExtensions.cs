using Xena.MemoryBus.Configurator;
using Xena.MemoryBus.Interfaces;
using Xena.Startup.Interfaces;

namespace Xena.MemoryBus;

public static class XenaMemoryBusServicesExtensions
{
    public static IXenaWebApplicationBuilder AddMemoryBus(
        this IXenaWebApplicationBuilder builder,
        Action<IXenaMemoryBusConfigurator>? configurationAction = null)
    {
        var configurator = new XenaMemoryBusConfigurator(builder);
        configurationAction?.Invoke(configurator);
        
        builder.Services.AddTransient<IXenaCommandBus, XenaCommandBus>();
        builder.Services.AddTransient<IXenaQueryBus, XenaQueryBus>();
        builder.Services.AddTransient<IXenaEventBus, XenaEventBus>();
        builder.Services.AddTransient<IXenaMemoryBus, XenaMemoryBus>();

        return builder;
    }
}