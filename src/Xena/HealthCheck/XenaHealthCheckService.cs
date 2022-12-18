using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Xena.HealthCheck;

public class XenaHealthCheckService : IHealthCheck
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public XenaHealthCheckService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();

        var xenaHealthChecks = scope.ServiceProvider.GetServices<IXenaHealthCheck>().ToList();

        var checksTasks = xenaHealthChecks
            .Where(h => h.Enabled)
            .Select(async xenaHealthCheck =>
            {
                var status = await xenaHealthCheck.Check(context, cancellationToken);

                var result = new
                {
                    status.Status,
                    xenaHealthCheck.Name
                };

                return result;
            })
            .ToList();

        var results = await Task.WhenAll(checksTasks);
        var resultsAsDictionary = results.ToDictionary(r => r.Name, r => (object)r);

        return results.Any(r => r.Status != HealthStatus.Healthy) 
            ? HealthCheckResult.Unhealthy("Xena Health check Unhealthy", data: resultsAsDictionary) 
            : HealthCheckResult.Healthy("Xena Health check Ok", resultsAsDictionary);
    }
}