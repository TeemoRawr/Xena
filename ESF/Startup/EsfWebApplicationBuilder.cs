namespace ESF.Startup;

internal sealed class EsfWebApplicationBuilder : IEsfWebApplicationBuilder
{
    public WebApplicationBuilder WebApplicationBuilder { get; }
    private readonly IList<Action<WebApplication>> _postBuildActions = new List<Action<WebApplication>>();

    public EsfWebApplicationBuilder(WebApplicationBuilder webApplicationBuilder)
    {
        WebApplicationBuilder = webApplicationBuilder;
    }

    public IEsfWebApplicationBuilder Configure(Action<WebApplicationBuilder> webApplicationBuilderAction)
    {
        webApplicationBuilderAction(WebApplicationBuilder);
        return this;
    }

    public IEsfWebApplicationBuilder AddPostBuildAction(Action<WebApplication> action)
    {
        _postBuildActions.Add(action);
        return this;
    }

    public EsfWebApplication Build()
    {
        var webApplication = WebApplicationBuilder.Build();

        foreach (var postBuildAction in _postBuildActions)
        {
            postBuildAction(webApplication);
        }

        var esfWebApplication = new EsfWebApplication(webApplication);

        return esfWebApplication;
    }
}