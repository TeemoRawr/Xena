using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xena.Discovery;
using Xena.Discovery.Configuration;
using Xena.HealthCheck;
using Xena.Startup.Interfaces;

namespace Xena.Tests.Discovery;

public class XenaDiscoveryServicesConfiguratorTests
{
    [Fact]
    public void AddHealthCheck_AddHealthCheckToServiceCollection()
    {
        // arrange
        var serviceCollectionMock = new Mock<IServiceCollection>();
        var xenaWebApplicationBuilderMock = new Mock<IXenaWebApplicationBuilder>();

        xenaWebApplicationBuilderMock.SetupGet(p => p.Services).Returns(serviceCollectionMock.Object);

        var sut = new XenaDiscoveryServicesConfigurator(xenaWebApplicationBuilderMock.Object);

        // act
        sut.AddHealthCheck();

        // assert
        serviceCollectionMock.Verify(p => p.Add(It.Is<ServiceDescriptor>(sd => 
            sd.Lifetime == ServiceLifetime.Scoped 
            && sd.ServiceType == typeof(IXenaHealthCheck)
            && sd.ImplementationType == typeof(XenaDiscoveryServicesHealthCheck))));
    }
}