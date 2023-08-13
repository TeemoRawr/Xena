using Xena.MemoryBus.Interfaces;

namespace Xena.Tests.MemoryBus.TestData;

public class SimpleCommandHandler : IXenaCommandHandler<SimpleCommand>
{
    public Task Handle(SimpleCommand command)
    {
        return Task.CompletedTask;
    }
}
