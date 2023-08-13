using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Xena.Discovery.Interfaces;

public interface IXenaDiscoveryHealthCheck
{
    Task<HealthCheckResult> Check(HealthCheckContext context, CancellationToken cancellationToken);
}