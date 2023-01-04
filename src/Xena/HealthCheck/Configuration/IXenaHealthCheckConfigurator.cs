namespace Xena.HealthCheck.Configuration;

public interface IXenaHealthCheckConfigurator
{
    /// <summary>
    /// Searches and registers automatically all services which implement <see cref="IXenaHealthCheck"/>  in dependency injection container as scoped
    /// </summary>
    /// <returns>
    /// The <see cref="IXenaHealthCheckConfigurator"/> so that additional calls can be chained
    /// </returns>
    IXenaHealthCheckConfigurator EnableAutoDiscoveryHealthChecks();
}