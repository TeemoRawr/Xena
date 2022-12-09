using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xena.MemoryBus;
using Xena.MemoryBus.Interfaces;

namespace Xena.Tests.MemoryBus;

public class QueryBusTests
{
    public class SimpleQuery : IXenaQuery<int>
    {
    }

    [Fact]
    public async Task Query_InvokeQueryHandler()
    {
        // arrange
        var fixture = new Fixture();

        var expectedResult = fixture.Create<int>();
        
        var query = new SimpleQuery();
        var queryHandlerMock = new Mock<IXenaQueryHandler<SimpleQuery, int>>();
        queryHandlerMock.Setup(p => p.Handle(It.IsAny<SimpleQuery>()))
            .ReturnsAsync(expectedResult);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(_ => queryHandlerMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var sut = new XenaQueryBus(serviceProvider);
        
        // act
        var result = await sut.Query(query);

        // assert
        queryHandlerMock.Verify(p => p.Handle(It.IsAny<SimpleQuery>()), Times.Once);
        result.Should().Be(expectedResult);
    }
}