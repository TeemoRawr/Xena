using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xena.HealthCheck;
using Xena.HealthCheck.Configuration;
using Xena.Startup;

namespace Xena.Tests.HealthCheck;

public class XenaHealthCheckConfiguratorTests 
{
    [Fact]
    public void EnableAutoDiscoveryHealthChecks_ShouldAutomaticallyRegisterAllServices()
    {
        // arrange
        var xenaWebApplicationBuilderMock = new Mock<IXenaWebApplicationBuilder>();

        var serviceCollectionMock = new Mock<IServiceCollection>();

        xenaWebApplicationBuilderMock.SetupGet(p => p.Services).Returns(serviceCollectionMock.Object);

        var sut = new XenaHealthCheckConfigurator(xenaWebApplicationBuilderMock.Object);

        // act
        sut.EnableAutoDiscoveryHealthChecks();

        // asserts
        serviceCollectionMock.Verify(p => p.Add(It.Is<ServiceDescriptor>(p => p.ImplementationType != null && p.ImplementationType.IsAssignableTo(typeof(IXenaHealthCheck)))));
    }
}