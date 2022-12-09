namespace Xena.MemoryBus.Interfaces;

public interface IXenaMemoryBus
{
    Task Send(IXenaCommand command);
    Task<TResult> Query<TResult>(IXenaQuery<TResult> query);
    Task Publish(IXenaEvent @event);
}