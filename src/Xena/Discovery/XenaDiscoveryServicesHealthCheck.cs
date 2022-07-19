using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xena.HealthCheck;

namespace Xena.Discovery;

public class XenaDiscoveryServicesHealthCheck : IXenaHealthCheck
{
    public string Name => "Xena Discovery services";
    public bool Enabled => true;

    public Task<HealthCheckResult> Check(HealthCheckContext context, CancellationToken cancellationToken)
    {
        var result = HealthCheckResult.Healthy();

        return Task.FromResult(result);
    }
}