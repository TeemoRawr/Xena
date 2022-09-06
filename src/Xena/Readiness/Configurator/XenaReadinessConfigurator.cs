using Xena.HealthCheck;
using Xena.Readiness.Interfaces;
using Xena.Startup;

namespace Xena.Readiness.Configurator;

internal class XenaReadinessConfigurator : IXenaReadinessConfigurator
{
    private readonly IXenaWebApplicationBuilder _xenaWebApplicationBuilder;

    public XenaReadinessConfigurator(IXenaWebApplicationBuilder xenaWebApplicationBuilder)
    {
        _xenaWebApplicationBuilder = xenaWebApplicationBuilder;
    }

    public IXenaReadinessConfigurator EnableAutoDiscoveryReadiness()
    {
        var xenaReadinessTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(p => p.GetTypes())
            .Where(t => typeof(IXenaReadiness).IsAssignableTo(t))
            .ToList();

        foreach (var xenaHealthCheckType in xenaReadinessTypes)
        {
            _xenaWebApplicationBuilder.WebApplicationBuilder.Services.AddScoped(
                typeof(IXenaHealthCheck),
                xenaHealthCheckType);
        }

        return this;
    }
}