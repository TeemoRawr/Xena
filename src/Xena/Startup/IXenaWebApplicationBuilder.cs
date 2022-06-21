namespace Xena.Startup;

public interface IXenaWebApplicationBuilder
{
    WebApplicationBuilder WebApplicationBuilder { get; }
    IXenaWebApplicationBuilder Configure(Action<WebApplicationBuilder> webApplicationBuilderAction);
    IXenaWebApplicationBuilder AddPostBuildAction(Action<WebApplication> action);
    XenaWebApplication Build();
}