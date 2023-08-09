using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Xena.Discovery;
using Xena.Startup.Interfaces;

namespace Xena.Tests.Discovery;

public class XenaDiscoveryServicesExtensionsTests
{
    [Fact]
    public void AddDiscovery_ShouldRegisterAllRequiredServices()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        
        var xenaWebApplicationBuilderMock = new Mock<IXenaWebApplicationBuilder>();
        xenaWebApplicationBuilderMock
            .SetupGet(p => p.Services)
            .Returns(serviceCollection);
        
        // act
        xenaWebApplicationBuilderMock.Object.AddDiscovery(configurator =>
        {
        });

        // assert

        serviceCollection.Should().HaveCount(2);
        serviceCollection.Should()
            .Contain(p =>
                p.ImplementationType == typeof(XenaDiscoveryStartupFilter) && p.ServiceType == typeof(IStartupFilter));
        
        serviceCollection.Should()
            .Contain(p =>
                p.ImplementationType == typeof(XenaDiscoverBackgroundService) && p.ServiceType == typeof(IHostedService));
        
    }
}