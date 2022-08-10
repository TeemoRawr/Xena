using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Refit;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;
using Xena.HttpClient.Factories;
using Xena.HttpClient.Models;

namespace Xena.HttpClient.Tests.Factories;

public class XenaHttpClientFactoryTests
{
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
        var services = _fixture.Build<Service>()
            .With(p => p.Name, typeof(ITestHttpClient).FullName)
            .With(p => p.Address, "localhost")
            .CreateMany(1)
            .ToList();

        var loggerMock = new Mock<ILogger<XenaHttpClientFactory>>();

        var xenaDiscoveryServicesServiceMock = new Mock<IXenaDiscoveryServicesService>();
        xenaDiscoveryServicesServiceMock.Setup(p => p.FindByTagAsync(It.IsAny<string>()))
            .ReturnsAsync(services);

        var sut = new XenaHttpClientFactory(xenaDiscoveryServicesServiceMock.Object, loggerMock.Object);

        // act
        var result = await sut.CreateHttpClient<ITestHttpClient>();

        // assert
        result.Should().BeAssignableTo<ITestHttpClient>();
        result.Should().BeAssignableTo<IXenaHttpClient>();
    }

    [Fact]
    public async Task CreateHttpClient_WhenTheClienIsRegisteredMoreThanOneTime_ShouldThrowAnException()
    {
        // arrange
        var services = _fixture.Build<Service>()
            .With(p => p.Name, typeof(ITestHttpClient).FullName)
            .With(p => p.Address, "localhost")
            .CreateMany()
            .ToList();

        var xenaDiscoveryServicesServiceMock = new Mock<IXenaDiscoveryServicesService>();
        xenaDiscoveryServicesServiceMock.Setup(p => p.FindByTagAsync(It.IsAny<string>()))
            .ReturnsAsync(services);

        var loggerMock = new Mock<ILogger<XenaHttpClientFactory>>();

        var sut = new XenaHttpClientFactory(xenaDiscoveryServicesServiceMock.Object, loggerMock.Object);

        // act and assert
        var exception = await Assert.ThrowsAsync<Exception>(() => sut.CreateHttpClient<ITestHttpClient>());
        exception.Message.Should()
            .Contain("Found more then one registered services for HttpClient");
    }

    [Fact]
    public async Task CreateHttpClient_WithoutAnyServices_ShouldThrowAnException()
    {
        // arrange

        var xenaDiscoveryServicesServiceMock = new Mock<IXenaDiscoveryServicesService>();
        xenaDiscoveryServicesServiceMock.Setup(p => p.FindByTagAsync(It.IsAny<string>()))
            .ReturnsAsync(Enumerable.Empty<Service>().ToList());

        var loggerMock = new Mock<ILogger<XenaHttpClientFactory>>();

        var sut = new XenaHttpClientFactory(xenaDiscoveryServicesServiceMock.Object, loggerMock.Object);

        // act and assert
        var exception = await Assert.ThrowsAsync<Exception>(() => sut.CreateHttpClient<ITestHttpClient>());
        exception.Message.Should()
            .Be($"Not found registered service for HttpClient {typeof(ITestHttpClient).FullName}");
    }
}