using Xena.Startup;

namespace Xena.HealthCheck.Configuration;

internal class XenaHealthCheckConfigurator : IXenaHealthCheckConfigurator
{
    private readonly IXenaWebApplicationBuilder _xenaWebApplicationBuilder;

    public XenaHealthCheckConfigurator(IXenaWebApplicationBuilder xenaWebApplicationBuilder)
    {
        _xenaWebApplicationBuilder = xenaWebApplicationBuilder;
    }

    public IXenaHealthCheckConfigurator EnableAutoDiscoveryHealthChecks()
    {
        var xenaHealthCheckTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(p => p.GetTypes())
            .Where(t => t.IsClass && t.IsAssignableTo(typeof(IXenaHealthCheck)))
            .ToList();

        foreach (var xenaHealthCheckType in xenaHealthCheckTypes)
        {
            _xenaWebApplicationBuilder.WebApplicationBuilder.Services.AddScoped(
                typeof(IXenaHealthCheck),
                xenaHealthCheckType);
        }

        return this;
    }
}