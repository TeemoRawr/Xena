namespace Xena.Startup;

public interface IXenaWebApplicationBuilder
{
    /// <summary>
    /// Application builder used by default by Asp.Net Core
    /// </summary>
    WebApplicationBuilder WebApplicationBuilder { get; }

    /// <summary>
    /// Method which allow to add action when the application will be builded. E.g you can use to invoke some service after build
    /// </summary>
    /// <param name="action">
    /// Callback which will be invoked when application will be builded
    /// </param>
    /// <returns>
    /// Allows fluent usage by returning the same builder
    /// </returns>
    IXenaWebApplicationBuilder AddPostBuildAction(Action<WebApplication> action);

    /// <summary>
    /// Build application
    /// </summary>
    /// <returns>
    /// Returns <see cref="XenaWebApplication"/>
    /// </returns>
    XenaWebApplication Build();
}