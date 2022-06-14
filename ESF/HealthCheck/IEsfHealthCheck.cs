using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ESF.HealthCheck;

public interface IEsfHealthCheck
{
    string Name { get; }
    bool Enabled { get; }
    Task<HealthCheckResult> Check(HealthCheckContext context, CancellationToken cancellationToken);
}