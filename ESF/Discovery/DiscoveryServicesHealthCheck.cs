using ESF.HealthCheck;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ESF.Discovery;

public class DiscoveryServicesHealthCheck : IEsfHealthCheck
{
    public string Name => "Discovery services";
    public bool Enabled => true;
    public Task<HealthCheckResult> Check(HealthCheckContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}