using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Xena.HealthCheck;

/// <summary>
/// Interface used internally by Xena framework which allows adding custom health check. All implementations of the interface are automatically discovered
/// </summary>
public interface IXenaHealthCheck
{
    /// <summary>
    /// Name of health check
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Allow enable or disable health check
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Method which allows adding action when the application is built. E.g you can use it to invoke some service after build
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<HealthCheckResult> Check(HealthCheckContext context, CancellationToken cancellationToken);
}