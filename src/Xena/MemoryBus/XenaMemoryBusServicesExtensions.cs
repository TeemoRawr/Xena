using Xena.MemoryBus.Interfaces;
using Xena.Startup.Interfaces;

namespace Xena.MemoryBus;

public static class XenaMemoryBusServicesExtensions
{
    public static IXenaWebApplicationBuilder AddMemoryBus(this IXenaWebApplicationBuilder builder)
    {
        builder.Services.AddTransient<XenaCommandBus>();
        builder.Services.AddTransient<XenaQueryBus>();
        builder.Services.AddTransient<XenaEventBus>();
        builder.Services.AddTransient<IXenaMemoryBus, XenaMemoryBus>();

        return builder;
    }
}