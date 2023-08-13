namespace Xena.MemoryBus.Interfaces
{
    public interface IXenaQueryBus
    {
        Task<TResult> Query<TResult>(IXenaQuery<TResult> query);
    }
}