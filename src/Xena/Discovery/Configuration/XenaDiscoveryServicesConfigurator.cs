using System.Diagnostics.CodeAnalysis;
using Xena.HealthCheck;
using Xena.Startup;

namespace Xena.Discovery.Configuration;

[ExcludeFromCodeCoverage]
internal class XenaDiscoveryServicesConfigurator : IXenaDiscoveryServicesConfigurator
{
    private readonly IXenaWebApplicationBuilder _xenaWebApplicationBuilder;

    public XenaDiscoveryServicesConfigurator(IXenaWebApplicationBuilder xenaWebApplicationBuilder)
    {
        _xenaWebApplicationBuilder = xenaWebApplicationBuilder;
    }

    public IServiceCollection ServiceCollection => _xenaWebApplicationBuilder.Services;

    public IXenaDiscoveryServicesConfigurator AddHealthCheck()
    {
        ServiceCollection.AddScoped<IXenaHealthCheck, XenaDiscoveryServicesHealthCheck>();

        return this;
    }

    public IXenaDiscoveryServicesConfigurator AddPostBuildAction(Action<WebApplication> action)
    {
        _xenaWebApplicationBuilder.AddPostBuildAction(action);
        return this;
    }
}