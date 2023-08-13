using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xena.MemoryBus.Configurator;
using Xena.MemoryBus.Interfaces;
using Xena.Startup.Interfaces;
using Xena.Tests.MemoryBus.TestData;

namespace Xena.Tests.MemoryBus;

public class XenaMemoryBusConfiguratorTests
{
    [Fact]
    public void EnableAutoDiscoveryCommands_ShouldRegisterAllCommandHandlers()
    {
        // arrange
        var serviceCollection = new ServiceCollection();

        var xenaWebApplicationBuilderMock = new Mock<IXenaWebApplicationBuilder>();
        xenaWebApplicationBuilderMock
            .SetupGet(p => p.Services)
            .Returns(serviceCollection);

        var sut = new XenaMemoryBusConfigurator(xenaWebApplicationBuilderMock.Object);

        // act
        sut.EnableAutoDiscoveryCommands();

        // assert
        serviceCollection.Should().Contain(p => p.ServiceType == typeof(IXenaCommandHandler<SimpleCommand>) && p.ImplementationType == typeof(SimpleCommandHandler));
    }

    [Fact]
    public void EnableAutoDiscoveryEvents_ShouldRegisterAllEventHandlers()
    {
        // arrange
        var serviceCollection = new ServiceCollection();

        var xenaWebApplicationBuilderMock = new Mock<IXenaWebApplicationBuilder>();
        xenaWebApplicationBuilderMock
            .SetupGet(p => p.Services)
            .Returns(serviceCollection);

        var sut = new XenaMemoryBusConfigurator(xenaWebApplicationBuilderMock.Object);

        // act
        sut.EnableAutoDiscoveryEvents();

        // assert
        serviceCollection.Should().Contain(p => p.ServiceType == typeof(IXenaEventHandler<SimpleEvent>) && p.ImplementationType == typeof(SimpleEventHandler));
    }

    [Fact]
    public void EnableAutoDiscoveryQueries_ShouldRegisterAllQueryHandlers()
    {
        // arrange
        var serviceCollection = new ServiceCollection();

        var xenaWebApplicationBuilderMock = new Mock<IXenaWebApplicationBuilder>();
        xenaWebApplicationBuilderMock
            .SetupGet(p => p.Services)
            .Returns(serviceCollection);

        var sut = new XenaMemoryBusConfigurator(xenaWebApplicationBuilderMock.Object);

        // act
        sut.EnableAutoDiscoveryQueries();

        // assert
        serviceCollection.Should().Contain(p => p.ServiceType == typeof(IXenaQueryHandler<SimpleQuery, int>) && p.ImplementationType == typeof(SimpleQueryHandle));
    }
}
