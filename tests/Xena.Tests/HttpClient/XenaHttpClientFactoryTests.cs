﻿using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Refit;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;
using Xena.HttpClient.Factories;
using Xena.HttpClient.Interceptors.Interfaces;
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
    public void CreateHttpClient_ShouldCreateImplementedHttpClient()
    {
        // arrange
        var service = new Service(
            "test",
            _fixture.Create<string>(),
            "localhost",
            _fixture.Create<int>());

        var loggerMock = new Mock<ILogger<XenaHttpClientFactory>>();

        var xenaDiscoveryServicesServiceMock = new Mock<IXenaDiscoveryProvider>();
        xenaDiscoveryServicesServiceMock.Setup(p => p.GetService(It.IsAny<string>()))
            .Returns(service);

        var sut = new XenaHttpClientFactory(
            xenaDiscoveryServicesServiceMock.Object, 
            new List<IXenaHttpClientInterceptor>(),
            loggerMock.Object);

        // act
        var result = sut.CreateHttpClient<ITestHttpClient>();

        // assert
        result.Should().BeAssignableTo<ITestHttpClient>();
        result.Should().BeAssignableTo<IXenaHttpClient>();
    }
    
    [Fact]
    public void CreateHttpClient_WithoutAnyServices_ShouldThrowAnException()
    {
        // arrange
        var xenaDiscoveryServicesServiceMock = new Mock<IXenaDiscoveryProvider>();

        var loggerMock = new Mock<ILogger<XenaHttpClientFactory>>();

        var sut = new XenaHttpClientFactory(
            xenaDiscoveryServicesServiceMock.Object,
            new List<IXenaHttpClientInterceptor>(), 
            loggerMock.Object);

        // act and assert
        Assert.Throws<Exception>(() => sut.CreateHttpClient<ITestHttpClient>());
    }

    [Fact]
    public void CreateHttpClient_WithoutServiceDiscovery_ShouldThrowAnExceptionOnInitialize()
    {
        // arrange
        var loggerMock = new Mock<ILogger<XenaHttpClientFactory>>();

        // act and assert
        var exception = Assert.Throws<NullReferenceException>(() => new XenaHttpClientFactory(
            null,
            new List<IXenaHttpClientInterceptor>(), 
            loggerMock.Object));
        exception.Message.Should()
            .Be($"Interface {nameof(IXenaDiscoveryProvider)} is not registered. " +
                "Please add Discovery module to application");
    }
}