namespace Xena.MemoryBus.Configurator;

public interface IXenaMemoryBusConfigurator
{
    IXenaMemoryBusConfigurator EnableAutoDiscoveryCommands();
    IXenaMemoryBusConfigurator EnableAutoDiscoveryEvents();
    IXenaMemoryBusConfigurator EnableAutoDiscoveryQueries();
}