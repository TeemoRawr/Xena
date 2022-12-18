using System.Net;
using AutoFixture;
using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xena.Discovery.Consul.Configuration;

namespace Xena.Discovery.Consul.Tests;

public class ConsulXenaDiscoveryProviderTests
{
    private readonly IFixture _fixture = new Fixture();

    [Fact]
    public async Task InitializeConsul_ShouldRegisterConsulAgent()
    {
        // arrange
        var expectedServiceId = _fixture.Create<string>();

        var serverAddressesFeatureMock = new Mock<IServerAddressesFeature>();
        serverAddressesFeatureMock.SetupGet(p => p.Addresses).Returns(new List<string>
        {
            "https://localhost:45075"
        });

        var featureCollectionMock = new Mock<IFeatureCollection>();
        featureCollectionMock.Setup(p => p.Get<IServerAddressesFeature>())
            .Returns(serverAddressesFeatureMock.Object);

        var serverMock = new Mock<IServer>();
        serverMock.SetupGet(p => p.Features).Returns(featureCollectionMock.Object);

        var agentEndpointMock = new Mock<IAgentEndpoint>();
        agentEndpointMock.Setup(p =>
                p.ServiceRegister(It.IsAny<AgentServiceRegistration>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new WriteResult
            {
                StatusCode = HttpStatusCode.OK,
                RequestTime = TimeSpan.FromSeconds(1)
            });

        var healthEndpointMock = new Mock<IHealthEndpoint>();
        healthEndpointMock.Setup(p => p.Service(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new QueryResult<ServiceEntry[]>
            {
                Response = new ServiceEntry[] { }
            });

        var consulClientMock = new Mock<IConsulClient>();
        consulClientMock.SetupGet(p => p.Agent).Returns(agentEndpointMock.Object);
        consulClientMock.SetupGet(p => p.Health).Returns(healthEndpointMock.Object);

        var consulXenaDiscoveryServicesConfiguration = _fixture.Build<ConsulXenaDiscoveryServicesConfiguration>()
            .With(p => p.Id, expectedServiceId)
            .Create();

        var consulXenaDiscoveryServicesConfigurationOptions = new OptionsWrapper<ConsulXenaDiscoveryServicesConfiguration>(consulXenaDiscoveryServicesConfiguration);

        var loggerMock = new Mock<ILogger<ConsulXenaDiscoveryProvider>>();

        var sut = new ConsulXenaDiscoveryProvider(
            serverMock.Object,
            consulXenaDiscoveryServicesConfigurationOptions,
            loggerMock.Object,
            consulClientMock.Object);

        // act
        await sut.InitializeConsulAsync();

        // assert
        agentEndpointMock
            .Verify(p => p.ServiceRegister(
                It.Is<AgentServiceRegistration>(p => p.ID == expectedServiceId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task InitializeConsul_WhenServerAddressesAreEmpty_ThrowInvalidOperationException(bool useEmptyAddresses)
    {
        // arrange
        var expectedServiceId = _fixture.Create<string>();

        IServerAddressesFeature? serverAddressesFeature = null; 
        if (useEmptyAddresses)
        {
            var serverAddressesFeatureMock = new Mock<IServerAddressesFeature>();
            serverAddressesFeatureMock.SetupGet(p => p.Addresses).Returns(new List<string>());
            serverAddressesFeature = serverAddressesFeatureMock.Object;
        }

        var featureCollectionMock = new Mock<IFeatureCollection>();
        featureCollectionMock.Setup(p => p.Get<IServerAddressesFeature>())
            .Returns(serverAddressesFeature);

        var serverMock = new Mock<IServer>();
        serverMock.SetupGet(p => p.Features).Returns(featureCollectionMock.Object);

        var agentEndpointMock = new Mock<IAgentEndpoint>();

        var healthEndpointMock = new Mock<IHealthEndpoint>();

        var consulClientMock = new Mock<IConsulClient>();
        consulClientMock.SetupGet(p => p.Agent).Returns(agentEndpointMock.Object);
        consulClientMock.SetupGet(p => p.Health).Returns(healthEndpointMock.Object);

        var consulXenaDiscoveryServicesConfiguration = _fixture.Build<ConsulXenaDiscoveryServicesConfiguration>()
            .With(p => p.Id, expectedServiceId)
            .Create();

        var consulXenaDiscoveryServicesConfigurationOptions = new OptionsWrapper<ConsulXenaDiscoveryServicesConfiguration>(consulXenaDiscoveryServicesConfiguration);

        var loggerMock = new Mock<ILogger<ConsulXenaDiscoveryProvider>>();

        var sut = new ConsulXenaDiscoveryProvider(
            serverMock.Object,
            consulXenaDiscoveryServicesConfigurationOptions,
            loggerMock.Object,
            consulClientMock.Object);

        // act & assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.InitializeConsulAsync());
    }
}