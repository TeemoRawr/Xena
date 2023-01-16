namespace Xena.MemoryBus.Interfaces;

public interface IXenaMemoryBus
{
    Task Send<TCommand>(TCommand command) where TCommand : IXenaCommand;
    Task<TResult> Query<TResult>(IXenaQuery<TResult> query);
    Task Publish<TEvent>(TEvent @event) where TEvent : IXenaEvent;
}