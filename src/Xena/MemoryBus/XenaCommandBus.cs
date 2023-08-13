using Xena.MemoryBus.Interfaces;

namespace Xena.MemoryBus;

internal class XenaCommandBus : IXenaCommandBus
{
    private readonly IServiceProvider _serviceProvider;

    public XenaCommandBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Send<TCommand>(TCommand command) where TCommand : IXenaCommand
    {
        var commandType = typeof(TCommand);
        var commandHandlerType = typeof(IXenaCommandHandler<>).MakeGenericType(commandType);

        var commandHandler = (IXenaCommandHandler<TCommand>)_serviceProvider.GetRequiredService(commandHandlerType);
        await commandHandler.Handle(command);
    }
}