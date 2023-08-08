using Xena.MemoryBus.Interfaces;

namespace Xena.MemoryBus;

internal class XenaMemoryBus : IXenaMemoryBus
{
    private readonly IXenaQueryBus _queryBus;
    private readonly IXenaCommandBus _commandBus;
    private readonly IXenaEventBus _eventBus;

    public XenaMemoryBus(
        IXenaQueryBus queryBus,
        IXenaCommandBus commandBus,
        IXenaEventBus eventBus)
    {
        _queryBus = queryBus;
        _commandBus = commandBus;
        _eventBus = eventBus;
    }

    public async Task Publish<TEvent>(TEvent @event) where TEvent : IXenaEvent
    {
        await _eventBus.Publish(@event);
    }

    public async Task Send<TCommand>(TCommand command) where TCommand : IXenaCommand
    {
        await _commandBus.Send(command);
    }

    public async Task<TResult> Query<TResult>(IXenaQuery<TResult> query)
    {
        var result = await _queryBus.Query(query);
        return result;
    }
}