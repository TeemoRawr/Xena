using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Xena.HealthCheck;

public interface IXenaHealthCheck
{
    string Name { get; }
    bool Enabled { get; }
    Task<HealthCheckResult> Check(HealthCheckContext context, CancellationToken cancellationToken);
}