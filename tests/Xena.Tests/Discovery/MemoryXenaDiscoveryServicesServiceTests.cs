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

        var sut = new MemoryXenaDiscoveryServicesService(emptyCollectionOfServices);

        // act
        await sut.AddServiceAsync(serviceToAdd);

        // assert
        var addedService = await sut.GetServiceAsync(serviceToAdd.Id);

        addedService.Should().BeEquivalentTo(serviceToAdd);
    }

    [Fact]
    public void ServiceIsNotInitialized_WhenServiceListIsEmpty()
    {
        // arrange 
        var emptyCollectionOfServices = Enumerable.Empty<Service>().ToList();

        var sut = new MemoryXenaDiscoveryServicesService(emptyCollectionOfServices);
        
        // assert
        sut.Initialized.Should().BeFalse();
    }

    [Fact]
    public void ServiceIsInitialized_WhenServiceListContainsSomeElements()
    {
        // arrange 
        var collectionOfServices = _fixture.CreateMany<Service>();

        var sut = new MemoryXenaDiscoveryServicesService(collectionOfServices);
        
        // assert
        sut.Initialized.Should().BeTrue();
    }
}