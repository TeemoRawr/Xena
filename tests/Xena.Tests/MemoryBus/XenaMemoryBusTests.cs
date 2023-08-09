using Moq;
using Xena.MemoryBus;
using Xena.MemoryBus.Interfaces;

namespace Xena.Tests.MemoryBus;

    
public class XenaMemoryBusTests
{
    private Mock<IXenaQueryBus> _queryBusMock = null!;
    private Mock<IXenaCommandBus> _commandBusMock = null!;
    private Mock<IXenaEventBus> _eventBusMock = null!;

    public class SimpleCommand : IXenaCommand
    {
    }

    public class SimpleEvent : IXenaEvent
    {
    }

    public class SimpleQuery : IXenaQuery<int>
    {
    }

    [Fact]
    public async Task Publish_ShouldInvokeEventBus()
    {
        // arrange
        var simpleEvent = new SimpleEvent();
        
        var sut = CreateSut();
        
        // act
        await sut.Publish(simpleEvent);

        // assert
        _eventBusMock.Verify(p => p.Publish(It.IsAny<IXenaEvent>()), Times.Once);
    }

    [Fact]
    public async Task Send_ShouldInvokeEventBus()
    {
        // arrange
        var simpleCommand = new SimpleCommand();
        
        var sut = CreateSut();
        
        // act
        await sut.Send(simpleCommand);

        // assert
        _commandBusMock.Verify(p => p.Send(It.IsAny<IXenaCommand>()), Times.Once);
    }

    [Fact]
    public async Task Query_ShouldInvokeEventBus()
    {
        // arrange
        var simpleQuery = new SimpleQuery();
        
        var sut = CreateSut();
        
        // act
        await sut.Query(simpleQuery);

        // assert
        _queryBusMock.Verify(p => p.Query(It.IsAny<IXenaQuery<int>>()), Times.Once);
    }

    private XenaMemoryBus CreateSut()
    {
        _queryBusMock = new Mock<IXenaQueryBus>();
        _commandBusMock = new Mock<IXenaCommandBus>();
        _eventBusMock = new Mock<IXenaEventBus>();

        return new XenaMemoryBus(
            _queryBusMock.Object,
            _commandBusMock.Object,
            _eventBusMock.Object);
    }
}
