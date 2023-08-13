using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xena.HttpClient;
using Xena.HttpClient.Factories;
using Xena.HttpClient.Interceptors;
using Xena.HttpClient.Interceptors.Interfaces;
using Xena.Startup.Interfaces;

namespace Xena.Tests.HttpClient;

public class XenaHttpClientServicesExtensionsTests
{
    [Fact]
    public void AddHttpClients_AddAllRequiredServices()
    {
        // arrange
        var serviceCollection = new ServiceCollection();

        var xenaWebApplicationBuilderMock = new Mock<IXenaWebApplicationBuilder>();
        xenaWebApplicationBuilderMock
            .SetupGet(p => p.Services)
            .Returns(serviceCollection);

        // act
        xenaWebApplicationBuilderMock.Object.AddHttpClients(p => { });

        // assert

        serviceCollection.Should().Contain(p => p.ServiceType == typeof(IXenaHttpClientInterceptor) && p.ImplementationType == typeof(LoggerXenaHttpClientInterceptor));
        serviceCollection.Should().Contain(p => p.ServiceType == typeof(IXenaHttpClientInterceptor) && p.ImplementationType == typeof(TimeoutRetryXenaHttpClientInterceptor));
        serviceCollection.Should().Contain(p => p.ServiceType == typeof(XenaHttpClientFactory));
    }
}
