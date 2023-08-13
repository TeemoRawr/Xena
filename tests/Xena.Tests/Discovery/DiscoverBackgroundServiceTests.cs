using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using Xena.Discovery;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;

namespace Xena.Tests.Discovery;

public class DiscoverBackgroundServiceTests
{
    private Mock<IXenaDiscoveryProvider> _discoveryServicesServiceMock = null!;
    private Mock<IXenaDiscoveryFinalizerService> _discoveryFinalizerServiceMock = null!;
    
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
    public async Task ExecuteAsync_ShouldInvokeFinalize()
    {
        // arrange
        var sut = CreateSut();
        using var cancellationTokenSource = new CancellationTokenSource();

        // act
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));
        await sut.StartAsync(cancellationTokenSource.Token);
        await sut.StopAsync(cancellationTokenSource.Token);

        // asserts
        _discoveryFinalizerServiceMock.Verify(p => p.FinalizeAsync(), Times.AtMostOnce);
    }

    private XenaDiscoverBackgroundService CreateSut()
    {
        _discoveryServicesServiceMock = new Mock<IXenaDiscoveryProvider>();
        _discoveryFinalizerServiceMock = new Mock<IXenaDiscoveryFinalizerService>();

        var xenaDiscoveryOptions = new XenaDiscoveryOptions();
        var wrappedOptions = Options.Create(xenaDiscoveryOptions);


        return new XenaDiscoverBackgroundService(
            _discoveryServicesServiceMock.Object,
            _discoveryFinalizerServiceMock.Object,
            wrappedOptions);
    }
}