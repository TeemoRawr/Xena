namespace ESF.Discovery.Configuration;

public interface IDiscoveryServicesConfigurator
{
    IServiceCollection ServiceCollection { get; }
    IDiscoveryServicesConfigurator AddHealthCheck();
    IDiscoveryServicesConfigurator AddPostBuildAction(Action<WebApplication> action);
}