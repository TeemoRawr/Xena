using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xena.Discovery.Configuration;
using Xena.Discovery.Memory;
using Xena.Discovery.Models;

namespace Xena.Tests.Discovery;

public class MemoryDiscoveryServicesExtensionsTests
{
    [Fact]
    public void AddMemoryProvider_ShouldRegisterAllRequiredServices()
    {
        // arrange
        var serviceCollection = new ServiceCollection();

        var xenaDiscoveryServicesConfiguratorMock = new Mock<IXenaDiscoveryServicesConfigurator>();
        xenaDiscoveryServicesConfiguratorMock.SetupGet(p => p.ServiceCollection).Returns(serviceCollection);

        var sut = xenaDiscoveryServicesConfiguratorMock.Object;

        // act
        sut.AddMemoryProvider(new List<Service>(0));

        // assert
        serviceCollection.Should().HaveCount(2);

        serviceCollection.Should().Contain(p => p.ServiceType == typeof(MemoryXenaDiscoveryProvider));
    }
}
