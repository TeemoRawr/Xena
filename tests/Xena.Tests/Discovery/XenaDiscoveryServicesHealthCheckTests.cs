using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xena.Discovery;
using Xena.Discovery.Interfaces;

namespace Xena.Tests.Discovery;

public class XenaDiscoveryServicesHealthCheckTests
{
    [Theory]
    [InlineData(true, HealthStatus.Healthy)]
    [InlineData(false, HealthStatus.Unhealthy)]
    public async Task Check_ReturnsExpectedStatus_DependsOnDiscoveryServiceInitialization(bool initialized, HealthStatus expectedHealthStatus)
    {
        // arrange
        var xenaInitializeDiscoveryServicesServiceMock = new Mock<IXenaInitializeDiscoveryServicesService>();
        xenaInitializeDiscoveryServicesServiceMock.SetupGet(p => p.Initialized).Returns(initialized);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_ => xenaInitializeDiscoveryServicesServiceMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

        var sut = new XenaDiscoveryServicesHealthCheck(serviceScopeFactory);

        var healthCheckContext = new HealthCheckContext();

        // act
        var result = await sut.Check(healthCheckContext, CancellationToken.None);

        // assert
        result.Status.Should().Be(expectedHealthStatus);
    }
}