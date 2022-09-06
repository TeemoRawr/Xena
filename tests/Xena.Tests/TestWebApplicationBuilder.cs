using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xena.Startup.Interfaces;

namespace Xena.Tests;

public class TestWebApplicationBuilder : IXenaWebApplicationBuilder
{
    private readonly List<Func<IXenaWebApplication, Task>> _postBuildActions = new();

    private readonly IServiceCollection _serviceCollection;
    private readonly IXenaWebApplication _xenaWebApplication;

    public TestWebApplicationBuilder(
        IServiceCollection serviceCollection, 
        IXenaWebApplication xenaWebApplication)
    {
        _serviceCollection = serviceCollection;
        _xenaWebApplication = xenaWebApplication;
    }

    public IWebHostEnvironment Environment => throw new NotImplementedException();
    public IServiceCollection Services => _serviceCollection;
    public ConfigurationManager Configuration => throw new NotImplementedException();
    public ILoggingBuilder Logging => throw new NotImplementedException();
    public ConfigureWebHostBuilder WebHost => throw new NotImplementedException();
    public ConfigureHostBuilder Host => throw new NotImplementedException();

    public IXenaWebApplicationBuilder AddPostBuildAction(Action<IXenaWebApplication> action)
    {
        _postBuildActions.Add(p =>
        {
            action(p);
            return Task.CompletedTask;
        });

        return this;
    }

    public IXenaWebApplicationBuilder AddPostBuildAsyncAction(Func<IXenaWebApplication, Task> action)
    {
        _postBuildActions.Add(action);
        return this;
    }

    public async Task<IXenaWebApplication> BuildAsync()
    {
        foreach (var postBuildAction in _postBuildActions)
        {
            await postBuildAction(_xenaWebApplication);
        }

        return _xenaWebApplication;
    }
}