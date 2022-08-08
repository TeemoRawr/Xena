using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Xena.HealthCheck;

/// <summary>
/// Interface used internal by Xena framework which allow add custom health check. All implementations of the interface are automatically discovered
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
    /// Method which is invoked each time when health check is invoked
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<HealthCheckResult> Check(HealthCheckContext context, CancellationToken cancellationToken);
}