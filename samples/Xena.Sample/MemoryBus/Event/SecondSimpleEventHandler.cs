using Xena.MemoryBus.Interfaces;

namespace Xena.Sample.MemoryBus.Event;

public class SecondSimpleEventHandler : IXenaEventHandler<SimpleEvent>
{
    public Task Handle(SimpleEvent @event)
    {
        return Task.CompletedTask;
    }
}