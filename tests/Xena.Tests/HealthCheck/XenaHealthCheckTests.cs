using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xena.HealthCheck;

namespace Xena.Tests.HealthCheck;

public class XenaHealthCheckTests
{
    private readonly IFixture _fixture = new Fixture();

    [Fact]
    public async Task CheckHealthAsync_WithoutXenaHealCheckServices_ShouldReturnHealthyStatus()
    {
        // arrange 
        var serviceCollection = new ServiceCollection();
        await using var serviceProvider = serviceCollection.BuildServiceProvider();
        var serviceScopeFactory = serviceProvider!.GetRequiredService<IServiceScopeFactory>();

        var healthCheckContext = new HealthCheckContext();
        var sut = new XenaHealthCheck(serviceScopeFactory);

        // act
        var result = await sut.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        // asserts
        result.Status.Should().Be(HealthStatus.Healthy);
    }

    [Fact]
    public async Task CheckHealthAsync_WithHealthyXenaHealCheckServices_ShouldReturnHealthyStatus()
    {
        // arrange 
        var xenaHealthCheckMock = new Mock<IXenaHealthCheck>();
        xenaHealthCheckMock
            .Setup(p => p.Check(It.IsAny<HealthCheckContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HealthCheckResult.Healthy());
        xenaHealthCheckMock.SetupGet(p => p.Name).Returns(_fixture.Create<string>());
        xenaHealthCheckMock.SetupGet(p => p.Enabled).Returns(true);
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_ => xenaHealthCheckMock.Object);
        await using var serviceProvider = serviceCollection.BuildServiceProvider();
        var serviceScopeFactory = serviceProvider!.GetRequiredService<IServiceScopeFactory>();

        var healthCheckContext = new HealthCheckContext();
        var sut = new XenaHealthCheck(serviceScopeFactory);

        // act
        var result = await sut.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        // asserts
        result.Status.Should().Be(HealthStatus.Healthy);
    }

    [Fact]
    public async Task CheckHealthAsync_WithHealthyXenaHealCheckServiceAndUnhealthyEsfHealCheckService_ShouldReturnUnhealthyStatus()
    {
        // arrange 
        var healthyXenaHealthCheckMock = new Mock<IXenaHealthCheck>();
        healthyXenaHealthCheckMock
            .Setup(p => p.Check(It.IsAny<HealthCheckContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HealthCheckResult.Healthy());
        healthyXenaHealthCheckMock.SetupGet(p => p.Name).Returns(_fixture.Create<string>());
        healthyXenaHealthCheckMock.SetupGet(p => p.Enabled).Returns(true);

        var unhealthyXenaHealthCheckMock = new Mock<IXenaHealthCheck>();
        unhealthyXenaHealthCheckMock
            .Setup(p => p.Check(It.IsAny<HealthCheckContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HealthCheckResult.Unhealthy());
        unhealthyXenaHealthCheckMock.SetupGet(p => p.Name).Returns(_fixture.Create<string>());
        unhealthyXenaHealthCheckMock.SetupGet(p => p.Enabled).Returns(true);
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_ => healthyXenaHealthCheckMock.Object);
        serviceCollection.AddSingleton(_ => unhealthyXenaHealthCheckMock.Object);
        await using var serviceProvider = serviceCollection.BuildServiceProvider();
        var serviceScopeFactory = serviceProvider!.GetRequiredService<IServiceScopeFactory>();

        var healthCheckContext = new HealthCheckContext();
        var sut = new XenaHealthCheck(serviceScopeFactory);

        // act
        var result = await sut.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        // asserts
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }
}