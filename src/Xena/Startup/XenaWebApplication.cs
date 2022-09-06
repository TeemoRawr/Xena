using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http.Features;
using Xena.Startup.Interfaces;

namespace Xena.Startup;

public sealed class XenaWebApplication : IXenaWebApplication
{
    private readonly WebApplication _webApplication;

    IFeatureCollection IApplicationBuilder.ServerFeatures
    {
        get
        {
            var applicationBuilder = _webApplication as IApplicationBuilder;
            return applicationBuilder.ServerFeatures;
        }
    }

    ICollection<EndpointDataSource> IEndpointRouteBuilder.DataSources
    {
        get
        {
            var endpointRouteBuilder = _webApplication as IEndpointRouteBuilder;
            return endpointRouteBuilder.DataSources;
        }
    }

    IServiceProvider IEndpointRouteBuilder.ServiceProvider => Services;

    IDictionary<string, object?> IApplicationBuilder.Properties
    {
        get
        {
            var applicationBuilder = _webApplication as IApplicationBuilder;
            return applicationBuilder.Properties;
        }
    }

    internal XenaWebApplication(WebApplication webApplication)
    {
        _webApplication = webApplication;
    }

    public IServiceProvider Services => _webApplication.Services;

    /// <summary>
    /// The application's configured <see cref="IConfiguration"/>.
    /// </summary>
    public IConfiguration Configuration => _webApplication.Configuration;

    /// <summary>
    /// The application's configured <see cref="IWebHostEnvironment"/>.
    /// </summary>
    public IWebHostEnvironment Environment => _webApplication.Environment;

    /// <summary>
    /// Allows consumers to be notified of application lifetime events.
    /// </summary>
    public IHostApplicationLifetime Lifetime => _webApplication.Lifetime;

    /// <summary>
    /// The default logger for the application.
    /// </summary>
    public ILogger Logger => _webApplication.Logger;

    /// <summary>
    /// The list of URLs that the HTTP server is bound to.
    /// </summary>
    public ICollection<string> Urls => _webApplication.Urls;

    IServiceProvider IApplicationBuilder.ApplicationServices
    {
        get
        {
            var applicationBuilder = _webApplication as IApplicationBuilder;
            return applicationBuilder.ApplicationServices;
        }
        set
        {
            var applicationBuilder = _webApplication as IApplicationBuilder;
            applicationBuilder.ApplicationServices = value;
        }
    }

    /// <summary>
    /// Start the application.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// A <see cref="Task"/> that represents the startup of the <see cref="Microsoft.AspNetCore.Builder.WebApplication"/>.
    /// Successful completion indicates the HTTP server is ready to accept new requests.
    /// </returns>
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _webApplication.Logger.LogInformation($"Application is going to start");
            return _webApplication.StartAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _webApplication.Logger.LogCritical(e, "Error occurred while running application");
            throw;
        }
    }

    /// <summary>
    /// Shuts down the application.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// A <see cref="Task"/> that represents the shutdown of the <see cref="Microsoft.AspNetCore.Builder.WebApplication"/>.
    /// Successful completion indicates that all the HTTP server has stopped.
    /// </returns>
    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return _webApplication.StopAsync(cancellationToken);
    }

    /// <summary>
    /// Runs an application and returns a Task that only completes when the token is triggered or shutdown is triggered.
    /// </summary>
    /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the entire runtime of the <see cref="Microsoft.AspNetCore.Builder.WebApplication"/> from startup to shutdown.
    /// </returns>
    public Task RunAsync(string? url = null) => _webApplication.RunAsync(url);

    /// <summary>
    /// Runs an application and block the calling thread until host shutdown.
    /// </summary>
    /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
    public void Run(string? url = null) => _webApplication.Run(url);

    /// <summary>
    /// Disposes the application.
    /// </summary>
    void IDisposable.Dispose()
    {
        var disposable = _webApplication as IDisposable;
        disposable.Dispose();
    }

    /// <summary>
    /// Disposes the application.
    /// </summary>
    public ValueTask DisposeAsync() => _webApplication.DisposeAsync();

    // REVIEW: Should this be wrapping another type?
    IApplicationBuilder IApplicationBuilder.New()
    {
        var applicationBuilder = _webApplication as IApplicationBuilder;
        return applicationBuilder.New();
    }

    IApplicationBuilder IApplicationBuilder.Use(Func<RequestDelegate, RequestDelegate> middleware)
    {
        var applicationBuilder = _webApplication as IApplicationBuilder;
        return applicationBuilder.Use(middleware);
    }

    IApplicationBuilder IEndpointRouteBuilder.CreateApplicationBuilder() => ((IApplicationBuilder)this).New();

    RequestDelegate IApplicationBuilder.Build()
    {
        var application = _webApplication as IApplicationBuilder;
        return application.Build();
    }
}