using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ESF.HealthCheck;

public class EsfHealthCheck : IHealthCheck
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EsfHealthCheck(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();

        var esfHealthChecks = scope.ServiceProvider.GetServices<IEsfHealthCheck>().ToList();

        var checksTasks = esfHealthChecks
            .Where(h => h.Enabled)
            .Select(async esfHealthCheck =>
            {
                var status = await esfHealthCheck.Check(context, cancellationToken);

                var result = new
                {
                    status.Status,
                    esfHealthCheck.Name
                };

                return result;
            })
            .ToList();

        var results = await Task.WhenAll(checksTasks);
        var resultsAsDictionary = results.ToDictionary(r => r.Name, r => (object)r);

        return results.Any(r => r.Status != HealthStatus.Healthy) 
            ? HealthCheckResult.Unhealthy("Esf Health check Unhealthy", data: resultsAsDictionary) 
            : HealthCheckResult.Healthy("Esf Health check Ok", resultsAsDictionary);
    }
}