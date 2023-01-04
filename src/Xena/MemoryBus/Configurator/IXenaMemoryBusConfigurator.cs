using Xena.MemoryBus.Interfaces;

namespace Xena.MemoryBus.Configurator;

public interface IXenaMemoryBusConfigurator
{
    /// <summary>
    /// Searches and registers automatically all services which implement <see cref="IXenaCommandHandler{TCommand}"/>  in dependency injection container as scoped
    /// </summary>
    /// <returns>
    /// The <see cref="IXenaMemoryBusConfigurator"/> so that additional calls can be chained
    /// </returns>
    IXenaMemoryBusConfigurator EnableAutoDiscoveryCommands();
    
    /// <summary>
    /// Searches and registers automatically all services which implement <see cref="IXenaEventHandler{TEvent}"/>  in dependency injection container as scoped
    /// </summary>
    /// <returns>
    /// The <see cref="IXenaMemoryBusConfigurator"/> so that additional calls can be chained
    /// </returns>
    IXenaMemoryBusConfigurator EnableAutoDiscoveryEvents();
    
    /// <summary>
    /// Searches and registers automatically all services which implement <see cref="IXenaQueryHandler{TQuery, TResult}"/>  in dependency injection container as scoped
    /// </summary>
    /// <returns>
    /// The <see cref="IXenaMemoryBusConfigurator"/> so that additional calls can be chained
    /// </returns>
    IXenaMemoryBusConfigurator EnableAutoDiscoveryQueries();
}