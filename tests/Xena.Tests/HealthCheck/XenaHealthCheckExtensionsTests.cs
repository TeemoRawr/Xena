using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xena.HealthCheck;
using Xena.Startup.Interfaces;

namespace Xena.Tests.HealthCheck;

public class XenaHealthCheckExtensionsTests
{
    [Fact]
    public void AddHealthChecks_ShouldAddHealthCheckService()
    {
        // arrange
        Mock<IXenaWebApplication> xenaWebApplicationMock = new ();
        Mock<IServiceCollection> serviceCollectionMock = new();

        var webApplicationBuilder = new TestWebApplicationBuilder(
            serviceCollectionMock.Object, 
            xenaWebApplicationMock.Object);
        
        // act
        webApplicationBuilder.AddHealthChecks();

        // assert
        serviceCollectionMock.Verify(p => p.Add(It.Is<ServiceDescriptor>(
            p => p.ImplementationType != null
                && p.ImplementationType.IsAssignableTo(typeof(HealthCheckService)))));
        
        serviceCollectionMock.Verify(p => p.Add(It.Is<ServiceDescriptor>(
            p => p.ImplementationType != null
                && p.ImplementationType.IsAssignableTo(typeof(XenaHealthCheckStartupFilter)))));
    }
}