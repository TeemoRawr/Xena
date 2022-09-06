using Xena.Startup.Interfaces;

namespace Xena.Startup;

internal sealed class XenaWebApplicationBuilder : IXenaWebApplicationBuilder
{
    private readonly WebApplicationBuilder _webApplicationBuilder;
    private readonly List<Func<IXenaWebApplication, Task>> _postBuildActions = new();

    public XenaWebApplicationBuilder(WebApplicationBuilder webApplicationBuilder)
    {
        _webApplicationBuilder = webApplicationBuilder;
    }

    public IWebHostEnvironment Environment => _webApplicationBuilder.Environment;
    public IServiceCollection Services => _webApplicationBuilder.Services;
    public ConfigurationManager Configuration => _webApplicationBuilder.Configuration;
    public ILoggingBuilder Logging => _webApplicationBuilder.Logging;
    public ConfigureWebHostBuilder WebHost => _webApplicationBuilder.WebHost;
    public ConfigureHostBuilder Host => _webApplicationBuilder.Host;

    public IXenaWebApplicationBuilder AddPostBuildAction(Action<IXenaWebApplication> action)
    {
        _postBuildActions.Add(application =>
        {
            action(application);
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
        var webApplication = _webApplicationBuilder.Build();
        var xenaWebApplication = new XenaWebApplication(webApplication);

        foreach (var postBuildAction in _postBuildActions)
        {
            await postBuildAction(xenaWebApplication);
        }

        return xenaWebApplication;
    }
}