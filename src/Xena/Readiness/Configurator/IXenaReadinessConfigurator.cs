using Xena.Readiness.Interfaces;

namespace Xena.Readiness.Configurator;

public interface IXenaReadinessConfigurator
{
    /// <summary>
    /// Searches and registers automatically all services which implement <see cref="IXenaReadiness"/>  in dependency injection container as scoped
    /// </summary>
    /// <returns>
    /// The <see cref="IXenaReadinessConfigurator"/> so that additional calls can be chained
    /// </returns>
    IXenaReadinessConfigurator EnableAutoDiscoveryReadiness();
}