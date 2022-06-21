using Xena.HealthCheck;
using Xena.Startup;

namespace Xena.Discovery.Configuration;

internal class DiscoveryServicesConfigurator : IDiscoveryServicesConfigurator
{
    private readonly IXenaWebApplicationBuilder _xenaWebApplicationBuilder;

    public DiscoveryServicesConfigurator(IXenaWebApplicationBuilder xenaWebApplicationBuilder)
    {
        _xenaWebApplicationBuilder = xenaWebApplicationBuilder;
    }

    public IServiceCollection ServiceCollection => _xenaWebApplicationBuilder.WebApplicationBuilder.Services;

    public IDiscoveryServicesConfigurator AddHealthCheck()
    {
        ServiceCollection.AddScoped<IXenaHealthCheck, DiscoveryServicesHealthCheck>();

        return this;
    }

    public IDiscoveryServicesConfigurator AddPostBuildAction(Action<WebApplication> action)
    {
        _xenaWebApplicationBuilder.AddPostBuildAction(action);
        return this;
    }
}