namespace Xena.MemoryBus.Interfaces
{
    public interface IXenaEventBus
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : IXenaEvent;
    }
}