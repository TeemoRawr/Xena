using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xena.Discovery;
using Xena.Discovery.Interfaces;

namespace Xena.Tests.Discovery;

public class XenaDiscoveryStartupFilterTests
{
    [Fact]
    public void Configure_ShouldInvokeInitialize_IfInitializeServiceIsRegistered()
    {
        // arrange
        var xenaDiscoveryInitializeService = new Mock<IXenaDiscoveryInitializeService>();
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(_ => xenaDiscoveryInitializeService.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var applicationBuilderMock = new Mock<IApplicationBuilder>();
        applicationBuilderMock
            .Setup(p => p.ApplicationServices)
            .Returns(serviceProvider);
        
        var sut = new XenaDiscoveryStartupFilter();

        // act
        var result = sut.Configure(builder => { });

        result(applicationBuilderMock.Object);

        // assert
        xenaDiscoveryInitializeService
            .Verify(p => p.InitializeAsync(), Times.Once);
    }
    
    [Fact]
    public void Configure_ShouldInvokeInitialize_IfInitializeServiceIsNotRegistered()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var applicationBuilderMock = new Mock<IApplicationBuilder>();
        applicationBuilderMock
            .Setup(p => p.ApplicationServices)
            .Returns(serviceProvider);
        
        var sut = new XenaDiscoveryStartupFilter();

        // act & assert
        var result = sut.Configure(builder => { });

        result(applicationBuilderMock.Object);
    }
}