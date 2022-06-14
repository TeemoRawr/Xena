namespace ESF.Startup;

public interface IEsfWebApplicationBuilder
{
    WebApplicationBuilder WebApplicationBuilder { get; }
    IEsfWebApplicationBuilder Configure(Action<WebApplicationBuilder> webApplicationBuilderAction);
    IEsfWebApplicationBuilder AddPostBuildAction(Action<WebApplication> action);
    EsfWebApplication Build();
}