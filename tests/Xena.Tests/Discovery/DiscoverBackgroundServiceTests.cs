using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using Xena.Discovery;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;

namespace Xena.Tests.Discovery;

public class DiscoverBackgroundServiceTests
{
    private Mock<IXenaDiscoveryServicesProvider> _discoveryServicesServiceMock = null!;
    private readonly IFixture _fixture = new Fixture();
    
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

    private XenaDiscoverBackgroundService CreateSut()
    {
        _discoveryServicesServiceMock = new Mock<IXenaDiscoveryServicesProvider>();

        var xenaDiscoveryOptions = new XenaDiscoveryOptions();
        var wrappedOptions = Options.Create(xenaDiscoveryOptions);


        return new XenaDiscoverBackgroundService(
            _discoveryServicesServiceMock.Object,
            wrappedOptions);
    }
}