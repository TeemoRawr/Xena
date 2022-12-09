using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xena.MemoryBus;
using Xena.MemoryBus.Interfaces;

namespace Xena.Tests.MemoryBus;

public class CommandBusTests
{
    public class SimpleCommand : IXenaCommand
    {
    }

    [Fact]
    public async Task Publish_InvokeCommandHandler()
    {
        // arrange
        var command = new SimpleCommand();
        var commandHandlerMock = new Mock<IXenaCommandHandler<SimpleCommand>>();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(_ => commandHandlerMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var commandBus = new XenaCommandBus(serviceProvider);
        
        // act
        await commandBus.Send(command);

        // assert
        commandHandlerMock.Verify(p => p.Handle(It.IsAny<SimpleCommand>()), Times.Once);
    }
}