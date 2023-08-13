using Xena.MemoryBus.Interfaces;

namespace Xena.Tests.MemoryBus.TestData;

public class SimpleEventHandler : IXenaEventHandler<SimpleEvent>
{
    public Task Handle(SimpleEvent @event)
    {
        return Task.CompletedTask;
    }
}