using Xena.MemoryBus.Interfaces;

namespace Xena.Sample.MemoryBus.Query;

public class SimpleQueryHandler : IXenaQueryHandler<SimpleQuery, int>
{
    public Task<int> Handle(SimpleQuery query)
    {
        return Task.FromResult(1);
    }
}