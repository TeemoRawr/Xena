using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xena.Readiness;
using Xena.Readiness.Interfaces;
using Xena.Readiness.Models;

namespace Xena.Tests.Readiness;

public class ReadinessServiceTests
{
    [Theory]
    [InlineData(
        XenaReadinessStatus.Successful, 
        XenaReadinessBehavior.LogInformation, 
        XenaReadinessBehavior.LogWarning, 
        XenaReadinessBehavior.LogCritical, 
        new [] { LogLevel.Information }, 
        false)]
    [InlineData(
        XenaReadinessStatus.Successful, 
        XenaReadinessBehavior.LogInformation | XenaReadinessBehavior.LogWarning, 
        XenaReadinessBehavior.LogWarning, 
        XenaReadinessBehavior.LogCritical, 
        new [] { LogLevel.Information, LogLevel.Warning }, 
        false)]
    [InlineData(
        XenaReadinessStatus.Warning, 
        XenaReadinessBehavior.LogWarning, 
        XenaReadinessBehavior.LogWarning, 
        XenaReadinessBehavior.LogCritical, 
        new [] { LogLevel.Warning }, 
        false)]
    [InlineData(
        XenaReadinessStatus.Error, 
        XenaReadinessBehavior.LogWarning, 
        XenaReadinessBehavior.LogWarning, 
        XenaReadinessBehavior.LogCritical | XenaReadinessBehavior.TerminateApplication, 
        new [] { LogLevel.Critical }, 
        true)]
    public async Task Check_WhenReadinessWhenReturnExpectedStatus_ShouldInvokeExpectedBehavior(
        XenaReadinessStatus status,
        XenaReadinessBehavior initialSuccessfulBehavior,
        XenaReadinessBehavior initialSuccessfulWarning,
        XenaReadinessBehavior initialSuccessfulError,
        IReadOnlyList<LogLevel> expectedLogLevels,
        bool applicationHasBeenTerminated)
    {
        // arrange
        var xenaReadinessOptions = new XenaReadinessOptions
        {
            BehaviorOnSuccessful = initialSuccessfulBehavior,
            BehaviorOnWarning = initialSuccessfulWarning,
            BehaviorOnError = initialSuccessfulError
        };
        var wrappedXenaReadinessOptions = new OptionsWrapper<XenaReadinessOptions>(xenaReadinessOptions);

        var loggerXenaReadinessServiceMock = new Mock<ILogger<XenaReadinessService>>();
        var hostApplicationLifetimeMock = new Mock<IHostApplicationLifetime>();

        var xenaReadinessMock = new Mock<IXenaReadiness>();
        xenaReadinessMock.Setup(p => p.CheckAsync(It.IsAny<IServiceProvider>()))
            .ReturnsAsync(status);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(_ => xenaReadinessMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

        var sut = new XenaReadinessService(
            serviceScopeFactory,
            loggerXenaReadinessServiceMock.Object,
            hostApplicationLifetimeMock.Object,
            wrappedXenaReadinessOptions);

        // act
        await sut.CheckReadiness();

        // assert

        foreach (var expectedLogLevel in expectedLogLevels)
        {
            loggerXenaReadinessServiceMock.Verify(p => p.Log(
                It.Is<LogLevel>(p => p == expectedLogLevel),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ));
        }

        if (applicationHasBeenTerminated)
        {
            hostApplicationLifetimeMock.Verify(p => p.StopApplication(), Times.Once);
        }
    }
}