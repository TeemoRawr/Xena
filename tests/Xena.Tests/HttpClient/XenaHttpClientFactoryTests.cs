using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Refit;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;
using Xena.HttpClient.Factories;
using Xena.HttpClient.Models;

namespace Xena.Tests.HttpClient;

public class XenaHttpClientFactoryTests
{
    [XenaHttpClient(Name = "test")]
    public interface ITestHttpClient : IXenaHttpClient
    {
        [Post("/")]
        Task Test();
    }
    
    private readonly IFixture _fixture = new Fixture();

    [Fact]
    public async Task CreateHttpClient_ShouldCreateImplementedHttpClient()
    {
        // arrange
        var service = new Service(
            "test",
            _fixture.Create<string>(),
            "localhost",
            _fixture.Create<int>(),
            new List<string>());

        var loggerMock = new Mock<ILogger<XenaHttpClientFactory>>();

        var xenaDiscoveryServicesServiceMock = new Mock<IXenaDiscoveryProvider>();
        xenaDiscoveryServicesServiceMock.Setup(p => p.GetServiceAsync(It.IsAny<string>()))
            .ReturnsAsync(service);

        var sut = new XenaHttpClientFactory(xenaDiscoveryServicesServiceMock.Object, loggerMock.Object);

        // act
        var result = await sut.CreateHttpClient<ITestHttpClient>();

        // assert
        result.Should().BeAssignableTo<ITestHttpClient>();
        result.Should().BeAssignableTo<IXenaHttpClient>();
    }
    
    [Fact]
    public async Task CreateHttpClient_WithoutAnyServices_ShouldThrowAnException()
    {
        // arrange
        var xenaDiscoveryServicesServiceMock = new Mock<IXenaDiscoveryProvider>();

        var loggerMock = new Mock<ILogger<XenaHttpClientFactory>>();

        var sut = new XenaHttpClientFactory(xenaDiscoveryServicesServiceMock.Object, loggerMock.Object);

        // act and assert
        await Assert.ThrowsAsync<Exception>(() => sut.CreateHttpClient<ITestHttpClient>());
    }

    [Fact]
    public void CreateHttpClient_WithoutServiceDiscovery_ShouldThrowAnExceptionOnInitialize()
    {
        // arrange
        var loggerMock = new Mock<ILogger<XenaHttpClientFactory>>();

        // act and assert
        var exception = Assert.Throws<NullReferenceException>(() => new XenaHttpClientFactory(null, loggerMock.Object));
        exception.Message.Should()
            .Be($"Interface {nameof(IXenaDiscoveryProvider)} is not registered. " +
                "Please add Discovery module to application");
    }
}