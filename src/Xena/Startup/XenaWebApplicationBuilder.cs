using System.Diagnostics.CodeAnalysis;

namespace Xena.Startup;

[ExcludeFromCodeCoverage]
internal sealed class XenaWebApplicationBuilder : IXenaWebApplicationBuilder
{
    public WebApplicationBuilder WebApplicationBuilder { get; }
    private readonly IList<Action<WebApplication>> _postBuildActions = new List<Action<WebApplication>>();

    public XenaWebApplicationBuilder(WebApplicationBuilder webApplicationBuilder)
    {
        WebApplicationBuilder = webApplicationBuilder;
    }

    public IXenaWebApplicationBuilder Configure(Action<WebApplicationBuilder> webApplicationBuilderAction)
    {
        webApplicationBuilderAction(WebApplicationBuilder);
        return this;
    }

    public IXenaWebApplicationBuilder AddPostBuildAction(Action<WebApplication> action)
    {
        _postBuildActions.Add(action);
        return this;
    }

    public XenaWebApplication Build()
    {
        var webApplication = WebApplicationBuilder.Build();

        foreach (var postBuildAction in _postBuildActions)
        {
            postBuildAction(webApplication);
        }

        var xenaWebApplication = new XenaWebApplication(webApplication);

        return xenaWebApplication;
    }
}