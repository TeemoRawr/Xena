namespace Xena.Startup;

internal sealed class XenaWebApplicationBuilder : IXenaWebApplicationBuilder
{
    private readonly WebApplicationBuilder _webApplicationBuilder;
    private readonly IList<Func<WebApplication, Task>> _postBuildActions = new List<Func<WebApplication, Task>>();

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

    public IXenaWebApplicationBuilder AddPostBuildAction(Action<WebApplication> action)
    {
        _postBuildActions.Add(application =>
        {
            action(application);
            return Task.CompletedTask;
        });

        return this;
    }

    public IXenaWebApplicationBuilder AddPostBuildAsyncAction(Func<WebApplication, Task> action)
    {
        _postBuildActions.Add(action);
        return this;
    }

    public async Task<XenaWebApplication> BuildAsync()
    {
        var webApplication = _webApplicationBuilder.Build();

        foreach (var postBuildAction in _postBuildActions)
        {
            await postBuildAction(webApplication);
        }

        var xenaWebApplication = new XenaWebApplication(webApplication);

        return xenaWebApplication;
    }
}