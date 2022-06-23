using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xena.Discovery.Interfaces;
using Xena.HealthCheck;

namespace Xena.Discovery;

public class XenaDiscoveryServicesHealthCheck : IXenaHealthCheck
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public XenaDiscoveryServicesHealthCheck(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public string Name => "Xena Discovery services";
    public bool Enabled => true;

    public Task<HealthCheckResult> Check(HealthCheckContext context, CancellationToken cancellationToken)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();

        var xenaInitializeDiscoveryServicesService = serviceScope.ServiceProvider.GetRequiredService<IXenaInitializeDiscoveryServicesService>();

        var result = xenaInitializeDiscoveryServicesService.Initialized
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy("Xena discovery service is not initialized");

        return Task.FromResult(result);
    }
}