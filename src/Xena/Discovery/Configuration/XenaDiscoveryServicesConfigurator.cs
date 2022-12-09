using System.Diagnostics.CodeAnalysis;
using Xena.HealthCheck;
using Xena.Startup.Interfaces;

namespace Xena.Discovery.Configuration;

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

    public IXenaDiscoveryServicesConfigurator AddPostBuildAction(Action<IXenaWebApplication> action)
    {
        _xenaWebApplicationBuilder.AddPostBuildAction(action);
        return this;
    }

    public IXenaDiscoveryServicesConfigurator AddPostBuildAsyncAction(Func<IXenaWebApplication, Task> action)
    {
        _xenaWebApplicationBuilder.AddPostBuildAsyncAction(action);
        return this;
    }
}