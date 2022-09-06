using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xena.Readiness.Configurator;
using Xena.Readiness.Interfaces;
using Xena.Readiness.Models;
using Xena.Startup.Interfaces;

namespace Xena.Tests.Readiness;

public class XenaReadinessConfiguratorTests
{
    private class TestReadiness : IXenaReadiness
    {
        public Task<XenaReadinessStatus> CheckAsync(IServiceProvider serviceScopeServiceProvider)
        {
            return Task.FromResult(XenaReadinessStatus.Successful);
        }
    }

    [Fact]
    public void EnableAutoDiscoveryHealthChecks_ShouldAutomaticallyRegisterAllServices()
    {
        // arrange
        var xenaWebApplicationBuilderMock = new Mock<IXenaWebApplicationBuilder>();

        var serviceCollectionMock = new Mock<IServiceCollection>();

        xenaWebApplicationBuilderMock.SetupGet(p => p.Services).Returns(serviceCollectionMock.Object);

        var sut = new XenaReadinessConfigurator(xenaWebApplicationBuilderMock.Object);

        // act
        sut.EnableAutoDiscoveryReadiness();

        // asserts
        serviceCollectionMock.Verify(p => p.Add(It.Is<ServiceDescriptor>(p => p.ImplementationType != null && p.ImplementationType.IsAssignableTo(typeof(IXenaReadiness)))));
    }
}