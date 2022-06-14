using AutoFixture;
using ESF.HealthCheck;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;

namespace ESF.Tests.HealthCheck;

public class EsfHealthCheckTests
{
    private readonly IFixture _fixture = new Fixture();

    [Fact]
    public async Task CheckHealthAsync_WithoutEsfHealCheckServices_ShouldReturnHealthyStatus()
    {
        // arrange 
        var serviceCollection = new ServiceCollection();
        await using var serviceProvider = serviceCollection.BuildServiceProvider();
        var serviceScopeFactory = serviceProvider!.GetRequiredService<IServiceScopeFactory>();

        var healthCheckContext = new HealthCheckContext();
        var sut = new EsfHealthCheck(serviceScopeFactory);

        // act
        var result = await sut.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        // asserts
        result.Status.Should().Be(HealthStatus.Healthy);
    }

    [Fact]
    public async Task CheckHealthAsync_WithHealthyEsfHealCheckServices_ShouldReturnHealthyStatus()
    {
        // arrange 
        var esfHealthCheckMock = new Mock<IEsfHealthCheck>();
        esfHealthCheckMock
            .Setup(p => p.Check(It.IsAny<HealthCheckContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HealthCheckResult.Healthy());
        esfHealthCheckMock.SetupGet(p => p.Name).Returns(_fixture.Create<string>());
        esfHealthCheckMock.SetupGet(p => p.Enabled).Returns(true);
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_ => esfHealthCheckMock.Object);
        await using var serviceProvider = serviceCollection.BuildServiceProvider();
        var serviceScopeFactory = serviceProvider!.GetRequiredService<IServiceScopeFactory>();

        var healthCheckContext = new HealthCheckContext();
        var sut = new EsfHealthCheck(serviceScopeFactory);

        // act
        var result = await sut.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        // asserts
        result.Status.Should().Be(HealthStatus.Healthy);
    }

    [Fact]
    public async Task CheckHealthAsync_WithHealthyEsfHealCheckServiceAndUnhealthyEsfHealCheckService_ShouldReturnUnhealthyStatus()
    {
        // arrange 
        var healthyEsfHealthCheckMock = new Mock<IEsfHealthCheck>();
        healthyEsfHealthCheckMock
            .Setup(p => p.Check(It.IsAny<HealthCheckContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HealthCheckResult.Healthy());
        healthyEsfHealthCheckMock.SetupGet(p => p.Name).Returns(_fixture.Create<string>());
        healthyEsfHealthCheckMock.SetupGet(p => p.Enabled).Returns(true);

        var unhealthyEsfHealthCheckMock = new Mock<IEsfHealthCheck>();
        unhealthyEsfHealthCheckMock
            .Setup(p => p.Check(It.IsAny<HealthCheckContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HealthCheckResult.Unhealthy());
        unhealthyEsfHealthCheckMock.SetupGet(p => p.Name).Returns(_fixture.Create<string>());
        unhealthyEsfHealthCheckMock.SetupGet(p => p.Enabled).Returns(true);
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_ => healthyEsfHealthCheckMock.Object);
        serviceCollection.AddSingleton(_ => unhealthyEsfHealthCheckMock.Object);
        await using var serviceProvider = serviceCollection.BuildServiceProvider();
        var serviceScopeFactory = serviceProvider!.GetRequiredService<IServiceScopeFactory>();

        var healthCheckContext = new HealthCheckContext();
        var sut = new EsfHealthCheck(serviceScopeFactory);

        // act
        var result = await sut.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        // asserts
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }
}