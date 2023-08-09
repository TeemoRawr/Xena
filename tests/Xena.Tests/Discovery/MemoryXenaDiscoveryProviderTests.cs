using FluentAssertions;
using Xena.Discovery.Memory;
using Xena.Discovery.Models;

namespace Xena.Tests.Discovery;

public class MemoryXenaDiscoveryProviderTests
{
    [Fact]
    public void GetService_ReturnsRequestedService()
    {
        // arrange

        var expectedService = new Service("test", "test", "test", 123);
        
        var sut = new MemoryXenaDiscoveryProvider(new List<Service>
        {
            expectedService
        });
        
        // act
        var service = sut.GetService("test");

        // assert
        service.Should().Be(expectedService);
    }
}