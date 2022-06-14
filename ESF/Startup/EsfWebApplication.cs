using Microsoft.AspNetCore.Http.Features;

namespace ESF.Startup;

public sealed class EsfWebApplication : IHost, IApplicationBuilder, IEndpointRouteBuilder, IAsyncDisposable
{
    internal WebApplication WebApplication { get; }

    IFeatureCollection IApplicationBuilder.ServerFeatures
    {
        get
        {
            var applicationBuilder = WebApplication as IApplicationBuilder;
            return applicationBuilder.ServerFeatures;
        }
    }

    ICollection<EndpointDataSource> IEndpointRouteBuilder.DataSources
    {
        get
        {
            var endpointRouteBuilder = WebApplication as IEndpointRouteBuilder;
            return endpointRouteBuilder.DataSources;
        }
    }

    IServiceProvider IEndpointRouteBuilder.ServiceProvider => Services;

    IDictionary<string, object?> IApplicationBuilder.Properties
    {
        get
        {
            var applicationBuilder = WebApplication as IApplicationBuilder;
            return applicationBuilder.Properties;
        }
    }


    internal EsfWebApplication(WebApplication webApplication)
    {
        WebApplication = webApplication;
    }

    public IServiceProvider Services => WebApplication.Services;

    /// <summary>
    /// The application's configured <see cref="IConfiguration"/>.
    /// </summary>
    public IConfiguration Configuration => WebApplication.Configuration;

    /// <summary>
    /// The application's configured <see cref="IWebHostEnvironment"/>.
    /// </summary>
    public IWebHostEnvironment Environment => WebApplication.Environment;

    /// <summary>
    /// Allows consumers to be notified of application lifetime events.
    /// </summary>
    public IHostApplicationLifetime Lifetime => WebApplication.Lifetime;

    /// <summary>
    /// The default logger for the application.
    /// </summary>
    public ILogger Logger => WebApplication.Logger;

    /// <summary>
    /// The list of URLs that the HTTP server is bound to.
    /// </summary>
    public ICollection<string> Urls => WebApplication.Urls;

    IServiceProvider IApplicationBuilder.ApplicationServices
    {
        get
        {
            var applicationBuilder = WebApplication as IApplicationBuilder;
            return applicationBuilder.ApplicationServices;
        }
        set
        {
            var applicationBuilder = WebApplication as IApplicationBuilder;
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
            WebApplication.Logger.LogInformation($"Application is going to start");
            return WebApplication.StartAsync(cancellationToken);
        }
        catch (Exception e)
        {
            WebApplication.Logger.LogCritical(e, "Error occurred while running application");
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
        return WebApplication.StopAsync(cancellationToken);
    }

    /// <summary>
    /// Runs an application and returns a Task that only completes when the token is triggered or shutdown is triggered.
    /// </summary>
    /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the entire runtime of the <see cref="Microsoft.AspNetCore.Builder.WebApplication"/> from startup to shutdown.
    /// </returns>
    public Task RunAsync(string? url = null) => WebApplication.RunAsync(url);

    /// <summary>
    /// Runs an application and block the calling thread until host shutdown.
    /// </summary>
    /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
    public void Run(string? url = null) => WebApplication.Run(url);

    /// <summary>
    /// Disposes the application.
    /// </summary>
    void IDisposable.Dispose()
    {
        var disposable = WebApplication as IDisposable;
        disposable.Dispose();
    }

    /// <summary>
    /// Disposes the application.
    /// </summary>
    public ValueTask DisposeAsync() => WebApplication.DisposeAsync();

    // REVIEW: Should this be wrapping another type?
    IApplicationBuilder IApplicationBuilder.New()
    {
        var applicationBuilder = WebApplication as IApplicationBuilder;
        return applicationBuilder.New();
    }

    IApplicationBuilder IApplicationBuilder.Use(Func<RequestDelegate, RequestDelegate> middleware)
    {
        var applicationBuilder = WebApplication as IApplicationBuilder;
        return applicationBuilder.Use(middleware);
    }

    IApplicationBuilder IEndpointRouteBuilder.CreateApplicationBuilder() => ((IApplicationBuilder)this).New();

    RequestDelegate IApplicationBuilder.Build()
    {
        var application = WebApplication as IApplicationBuilder;
        return application.Build();
    }
}