namespace Xena.MemoryBus.Interfaces;

public interface IXenaEventHandler<in TEvent> where TEvent : IXenaEvent
{
    Task Handle(TEvent @event);
}