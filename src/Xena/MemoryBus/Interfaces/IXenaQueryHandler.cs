namespace Xena.MemoryBus.Interfaces;

public interface IXenaQueryHandler<TQuery, TResult> where TQuery : IXenaQuery<TResult>
{
    Task<TResult> Handle(TQuery query);
}