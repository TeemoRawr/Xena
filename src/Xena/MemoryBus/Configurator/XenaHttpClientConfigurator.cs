using System.Diagnostics.CodeAnalysis;
using Xena.MemoryBus.Interfaces;
using Xena.Startup.Interfaces;

namespace Xena.MemoryBus.Configurator;

[ExcludeFromCodeCoverage]
internal class XenaMemoryBusConfigurator : IXenaMemoryBusConfigurator
{
    private readonly IXenaWebApplicationBuilder _xenaWebApplicationBuilder;

    public XenaMemoryBusConfigurator(IXenaWebApplicationBuilder xenaWebApplicationBuilder)
    {
        _xenaWebApplicationBuilder = xenaWebApplicationBuilder;
    }

    public IXenaMemoryBusConfigurator EnableAutoDiscoveryCommands()
    {
        RegisterTypes(typeof(IXenaCommandHandler<>));
        return this;
    }

    public IXenaMemoryBusConfigurator EnableAutoDiscoveryEvents()
    {
        RegisterTypes(typeof(IXenaEventHandler<>));
        return this;
    }

    public IXenaMemoryBusConfigurator EnableAutoDiscoveryQueries()
    {
        RegisterTypes(typeof(IXenaQueryHandler<,>));
        return this;
    }

    private void RegisterTypes(Type searchingType)
    {
        var compatibleTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(p => p.GetTypes())
            .Where(t => t.IsClass && t.GetInterfaces()
                .Any(i =>  i.IsGenericType && 
                           i.GetGenericTypeDefinition().IsAssignableTo(searchingType)))
            .ToList();

        foreach (var compatibleType in compatibleTypes)
        {
            var interfaceTypes = compatibleType
                                     .GetInterfaces()
                                     .Where(i => i.GetGenericTypeDefinition().IsAssignableTo(searchingType))
                                     .ToList();

            foreach (var interfaceType in interfaceTypes)
            {
                _xenaWebApplicationBuilder.Services.AddScoped(
                    interfaceType,
                    compatibleType
                );
            }
        }
    }
}