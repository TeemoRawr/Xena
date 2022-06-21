using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xena.HealthCheck;

namespace Xena.Discovery;

public class DiscoveryServicesHealthCheck : IXenaHealthCheck
{
    public string Name => "Discovery services";
    public bool Enabled => true;
    public Task<HealthCheckResult> Check(HealthCheckContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}