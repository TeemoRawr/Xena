using Xena.Startup.Interfaces;

namespace Xena.Discovery.Configuration;

public interface IXenaDiscoveryServicesConfigurator
{
    IServiceCollection ServiceCollection { get; }
    IXenaDiscoveryServicesConfigurator AddHealthCheck();
    IXenaDiscoveryServicesConfigurator AddPostBuildAction(Action<IXenaWebApplication> action);
    IXenaDiscoveryServicesConfigurator AddPostBuildAsyncAction(Func<IXenaWebApplication, Task> action);
}