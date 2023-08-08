namespace Xena.MemoryBus.Interfaces
{
    public interface IXenaCommandBus
    {
        Task Send<TCommand>(TCommand command) where TCommand : IXenaCommand;
    }
}