using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xena.HealthCheck;

namespace Xena.Sample.HealthCheck;

public class BasicHealthCheck : IXenaHealthCheck
{
    public string Name => "Basic";
    public bool Enabled => true;
    public Task<HealthCheckResult> Check(HealthCheckContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}