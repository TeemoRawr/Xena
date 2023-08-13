using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics.CodeAnalysis;
using Xena.Startup.Interfaces;

namespace Xena.Startup;

[ExcludeFromCodeCoverage]
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
    
    public IConfiguration Configuration => _webApplication.Configuration;
    
    public IWebHostEnvironment Environment => _webApplication.Environment;
    
    public IHostApplicationLifetime Lifetime => _webApplication.Lifetime;
    
    public ILogger Logger => _webApplication.Logger;
    
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

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return _webApplication.StopAsync(cancellationToken);
    }
    
    public Task RunAsync(string? url = null) => _webApplication.RunAsync(url);
    
    public void Run(string? url = null) => _webApplication.Run(url);
    
    void IDisposable.Dispose()
    {
        var disposable = _webApplication as IDisposable;
        disposable.Dispose();
    }

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