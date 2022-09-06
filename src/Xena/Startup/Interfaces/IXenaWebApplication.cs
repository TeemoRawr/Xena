namespace Xena.Startup.Interfaces;

public interface IXenaWebApplication : IHost, IApplicationBuilder, IEndpointRouteBuilder, IAsyncDisposable
{
}