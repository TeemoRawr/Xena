using AutoFixture;
using FluentAssertions;
using Xena.Discovery.Memory;
using Xena.Discovery.Models;

namespace Xena.Tests.Discovery;

public class MemoryXenaDiscoveryServicesServiceTests
{
    private readonly IFixture _fixture = new Fixture();

    [Fact]
    public async Task AddServiceAsync_AddServices()
    {
        // arrange 
        var emptyCollectionOfServices = Enumerable.Empty<Service>().ToList();
        var serviceToAdd = _fixture.Create<Service>();

        var sut = new MemoryXenaDiscoveryServicesProvider(emptyCollectionOfServices);

        // act
        await sut.AddServiceAsync(serviceToAdd);

        // assert
        var addedService = await sut.GetServiceAsync(serviceToAdd.Id);

        addedService.Should().BeEquivalentTo(serviceToAdd);
    }
}