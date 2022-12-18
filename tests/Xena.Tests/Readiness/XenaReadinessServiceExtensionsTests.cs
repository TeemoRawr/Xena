using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xena.Readiness;
using Xena.Readiness.Models;
using Xena.Startup.Interfaces;

namespace Xena.Tests.Readiness;

public class XenaReadinessServiceExtensionsTests
{
    [Fact]
    public async Task AddReadiness_ShouldAddReadinessService()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(_ => new Mock<ILogger<XenaReadinessService>>().Object);
        serviceCollection.AddTransient(_ => new Mock<IHostApplicationLifetime>().Object);
        serviceCollection.AddTransient<IOptions<XenaReadinessOptions>>(_ => new OptionsWrapper<XenaReadinessOptions>(new XenaReadinessOptions()));

        var xenaWebApplicationMock = new Mock<IXenaWebApplication>();
        
        xenaWebApplicationMock.SetupGet(p => p.Services)
            .Returns(() => serviceCollection.BuildServiceProvider());

        var webApplicationBuilder = new TestWebApplicationBuilder(serviceCollection, xenaWebApplicationMock.Object);

        // act
        await webApplicationBuilder.AddReadiness().BuildAsync();

        // assert
        serviceCollection.Should().Contain(p => 
            p.ImplementationType != null
            && p.ImplementationType.IsAssignableTo(typeof(XenaReadinessService)));
    }
}