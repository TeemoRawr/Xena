using Moq;
using Xena.Discovery;
using Xena.Discovery.Interfaces;

namespace Xena.Tests.Discovery;

public class DiscoverBackgroundServiceTests
{
    private Mock<IXenaInitializeDiscoveryServicesService> _initializeDiscoveryServicesServiceMock = null!;
    private Mock<IXenaDiscoveryServicesService> _discoveryServicesServiceMock = null!;

    [Fact]
    public async Task ExecuteAsync_ShouldInvokeInitialize()
    {
        // arrange
        var sut = CreateSut();
        using var cancellationTokenSource = new CancellationTokenSource();

        // act
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));
        await sut.StartAsync(cancellationTokenSource.Token);
        await sut.StopAsync(cancellationTokenSource.Token);

        // asserts
        _initializeDiscoveryServicesServiceMock.Verify(p => p.InitializeAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldInvokeRefreshServices()
    {
        // arrange
        var sut = CreateSut();
        using var cancellationTokenSource = new CancellationTokenSource();

        // act
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));
        await sut.StartAsync(cancellationTokenSource.Token);
        await sut.StopAsync(cancellationTokenSource.Token);

        // asserts
        _discoveryServicesServiceMock.Verify(p => p.RefreshServicesAsync(It.IsAny<CancellationToken>()), Times.AtMostOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldInvokeDeactivate()
    {
        // arrange
        var sut = CreateSut();
        using var cancellationTokenSource = new CancellationTokenSource();

        // act
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));
        await sut.StartAsync(cancellationTokenSource.Token);
        await sut.StopAsync(cancellationTokenSource.Token);

        // asserts
        _initializeDiscoveryServicesServiceMock.Verify(p => p.DeactivateAsync(), Times.Once);
    }

    private DiscoverBackgroundService CreateSut()
    {
        _initializeDiscoveryServicesServiceMock = new Mock<IXenaInitializeDiscoveryServicesService>();
        _discoveryServicesServiceMock = new Mock<IXenaDiscoveryServicesService>();

        return new DiscoverBackgroundService(
            _initializeDiscoveryServicesServiceMock.Object,
            _discoveryServicesServiceMock.Object);
    }
}