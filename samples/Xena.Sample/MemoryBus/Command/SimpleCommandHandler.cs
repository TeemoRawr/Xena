using Xena.MemoryBus.Interfaces;

namespace Xena.Sample.MemoryBus.Command;

public class SimpleCommandHandler : IXenaCommandHandler<SimpleCommand>
{
    public Task Handle(SimpleCommand command)
    {
        return Task.CompletedTask;
    }
}