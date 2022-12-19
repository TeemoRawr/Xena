using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xena.Discovery.Interfaces;
using Xena.HealthCheck;

namespace Xena.Discovery;

public class XenaDiscoveryServicesHealthCheck : IXenaHealthCheck
{
    private readonly IXenaDiscoveryHealthCheck? _xenaDiscoveryHealthCheck;

    public XenaDiscoveryServicesHealthCheck(IXenaDiscoveryHealthCheck? xenaDiscoveryHealthCheck)
    {
        _xenaDiscoveryHealthCheck = xenaDiscoveryHealthCheck;
    }

    public string Name => "Xena Discovery services";
    public bool Enabled => true;

    public async Task<HealthCheckResult> Check(HealthCheckContext context, CancellationToken cancellationToken)
    {
        if (_xenaDiscoveryHealthCheck is null)
        {
            return HealthCheckResult.Healthy("Discovery health check is disabled"); 
        }

        var result = await _xenaDiscoveryHealthCheck.Check(context, cancellationToken);

        return result;
    }
}