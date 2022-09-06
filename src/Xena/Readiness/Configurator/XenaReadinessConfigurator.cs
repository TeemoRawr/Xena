using Xena.HealthCheck;
using Xena.Readiness.Interfaces;
using Xena.Startup.Interfaces;

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
            .Where(t => t.IsClass && t.IsAssignableTo(typeof(IXenaReadiness)))
            .ToList();

        foreach (var xenaReadinessType in xenaReadinessTypes)
        {
            _xenaWebApplicationBuilder.Services.AddScoped(
                typeof(IXenaReadiness),
                xenaReadinessType);
        }

        return this;
    }
}