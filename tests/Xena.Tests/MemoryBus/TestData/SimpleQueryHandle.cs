using Xena.MemoryBus.Interfaces;

namespace Xena.Tests.MemoryBus.TestData;

public class SimpleQueryHandle : IXenaQueryHandler<SimpleQuery, int>
{
    public Task<int> Handle(SimpleQuery query)
    {
        return Task.FromResult(0);
    }
}