namespace Xena.MemoryBus.Interfaces;

public interface IXenaCommandHandler<in TCommand> where TCommand : IXenaCommand
{
    Task Handle(TCommand command);
}