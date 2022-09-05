using System.Diagnostics.CodeAnalysis;

namespace Xena.Startup;

[ExcludeFromCodeCoverage]
internal sealed class XenaWebApplicationBuilder : IXenaWebApplicationBuilder
{
    public WebApplicationBuilder WebApplicationBuilder { get; }
    private readonly IList<Func<WebApplication, Task>> _postBuildActions = new List<Func<WebApplication, Task>>();

    public XenaWebApplicationBuilder(WebApplicationBuilder webApplicationBuilder)
    {
        WebApplicationBuilder = webApplicationBuilder;
    }

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
        var webApplication = WebApplicationBuilder.Build();

        foreach (var postBuildAction in _postBuildActions)
        {
            await postBuildAction(webApplication);
        }

        var xenaWebApplication = new XenaWebApplication(webApplication);

        return xenaWebApplication;
    }
}