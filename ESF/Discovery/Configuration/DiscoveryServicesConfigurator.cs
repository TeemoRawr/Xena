using ESF.HealthCheck;
using ESF.Startup;

namespace ESF.Discovery.Configuration;

internal class DiscoveryServicesConfigurator : IDiscoveryServicesConfigurator
{
    private readonly IEsfWebApplicationBuilder _esfWebApplicationBuilder;

    public DiscoveryServicesConfigurator(IEsfWebApplicationBuilder esfWebApplicationBuilder)
    {
        _esfWebApplicationBuilder = esfWebApplicationBuilder;
    }

    public IServiceCollection ServiceCollection => _esfWebApplicationBuilder.WebApplicationBuilder.Services;

    public IDiscoveryServicesConfigurator AddHealthCheck()
    {
        ServiceCollection.AddScoped<IEsfHealthCheck, DiscoveryServicesHealthCheck>();

        return this;
    }

    public IDiscoveryServicesConfigurator AddPostBuildAction(Action<WebApplication> action)
    {
        _esfWebApplicationBuilder.AddPostBuildAction(action);
        return this;
    }
}