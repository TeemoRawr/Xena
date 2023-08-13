using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xena.MemoryBus;
using Xena.MemoryBus.Interfaces;
using Xena.Tests.MemoryBus.TestData;

namespace Xena.Tests.MemoryBus;

public class EventBusTests
{   
    [Fact]
    public async Task Publish_ShouldInvokeAllEventHandlers()
    {
        // arrange
        var @event = new SimpleEvent();
        var eventHandlerOne = new Mock<IXenaEventHandler<SimpleEvent>>();
        var eventHandlerTwo = new Mock<IXenaEventHandler<SimpleEvent>>();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(_ => eventHandlerOne.Object);
        serviceCollection.AddTransient(_ => eventHandlerTwo.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var sut = new XenaEventBus(serviceProvider);

        // act
        await sut.Publish(@event);

        // assert
        eventHandlerOne.Verify(s => s.Handle(It.IsAny<SimpleEvent>()), Times.Once);
        eventHandlerTwo.Verify(s => s.Handle(It.IsAny<SimpleEvent>()), Times.Once);
    }
}