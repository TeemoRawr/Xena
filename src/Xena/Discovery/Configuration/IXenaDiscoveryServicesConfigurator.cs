namespace Xena.Discovery.Configuration;

public interface IXenaDiscoveryServicesConfigurator
{
    IServiceCollection ServiceCollection { get; }
    
    IXenaDiscoveryServicesConfigurator AddHealthCheck();
}