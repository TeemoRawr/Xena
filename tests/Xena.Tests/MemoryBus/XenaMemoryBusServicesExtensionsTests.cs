using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xena.MemoryBus;
using Xena.MemoryBus.Interfaces;
using Xena.Startup.Interfaces;

namespace Xena.Tests.MemoryBus;

public class XenaMemoryBusServicesExtensionsTests
{
    [Fact]
    public void AddMemoryBus_ShouldRegisterAllRequiredServices()
    {        
        // arrange
        var serviceCollection = new ServiceCollection();
        
        var xenaWebApplicationBuilderMock = new Mock<IXenaWebApplicationBuilder>();
        xenaWebApplicationBuilderMock.SetupGet(p => p.Services).Returns(serviceCollection);

        var sut = xenaWebApplicationBuilderMock.Object;

        // act
        sut.AddMemoryBus();

        // assert
        serviceCollection.Should().HaveCount(4);

        serviceCollection.Should().Contain(p => p.ImplementationType == typeof(XenaCommandBus) && p.ServiceType == typeof(IXenaCommandBus));
        serviceCollection.Should().Contain(p => p.ImplementationType == typeof(XenaQueryBus) && p.ServiceType == typeof(IXenaQueryBus));
        serviceCollection.Should().Contain(p => p.ImplementationType == typeof(XenaEventBus) && p.ServiceType == typeof(IXenaEventBus));
        serviceCollection.Should().Contain(p => p.ImplementationType == typeof(XenaMemoryBus) && p.ServiceType == typeof(IXenaMemoryBus));
    }
}
