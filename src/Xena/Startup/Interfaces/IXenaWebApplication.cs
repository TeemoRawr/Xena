namespace Xena.Startup.Interfaces;

public interface IXenaWebApplication : IHost, IApplicationBuilder, IEndpointRouteBuilder, IAsyncDisposable
{
    /// <summary>
    /// The application's configured <see cref="IConfiguration"/>.
    /// </summary>
    IConfiguration Configuration { get; }

    /// <summary>
    /// The application's configured <see cref="IWebHostEnvironment"/>.
    /// </summary>
    IWebHostEnvironment Environment { get; }

    /// <summary>
    /// Allows consumers to be notified of application lifetime events.
    /// </summary>
    IHostApplicationLifetime Lifetime { get; }

    /// <summary>
    /// The default logger for the application.
    /// </summary>
    ILogger Logger { get; }

    /// <summary>
    /// The list of URLs that the HTTP server is bound to.
    /// </summary>
    ICollection<string> Urls { get; }
}