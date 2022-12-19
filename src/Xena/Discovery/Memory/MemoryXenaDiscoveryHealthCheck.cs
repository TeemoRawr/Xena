using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xena.Discovery.Interfaces;

namespace Xena.Discovery.Memory;

internal class MemoryXenaDiscoveryHealthCheck : IXenaDiscoveryHealthCheck
{
    public Task<HealthCheckResult> Check(HealthCheckContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}