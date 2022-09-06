namespace Xena.Startup.Interfaces;

public interface IXenaWebApplicationBuilder
{
    /// <summary>
    /// Provides information about the web hosting environment an application is running.
    /// </summary>
    IWebHostEnvironment Environment { get; }

    /// <summary>
    /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
    /// </summary>
    ConfigurationManager Configuration { get; }

    /// <summary>
    /// A collection of logging providers for the application to compose. This is useful for adding new logging providers.
    /// </summary>
    ILoggingBuilder Logging { get; }

    /// <summary>
    /// An <see cref="IWebHostBuilder"/> for configuring server specific properties, but not building.
    /// To build after configuration, call <see cref="Build"/>.
    /// </summary>
    ConfigureWebHostBuilder WebHost { get; }

    /// <summary>
    /// An <see cref="IHostBuilder"/> for configuring host specific properties, but not building.
    /// To build after configuration, call <see cref="Build"/>.
    /// </summary>
    ConfigureHostBuilder Host { get; }

    /// <summary>
    /// Method which allow to add action when the application will be builded. E.g you can use to invoke some service after build
    /// </summary>
    /// <param name="action">
    /// Callback which will be invoked when application will be builded
    /// </param>
    /// <returns>
    /// Allows fluent usage by returning the same builder
    /// </returns>
    IXenaWebApplicationBuilder AddPostBuildAction(Action<IXenaWebApplication> action);

    /// <summary>
    /// Method which allow to add async action when the application will be builded. E.g you can use to invoke some service after build
    /// </summary>
    /// <param name="action">
    /// Callback which will be invoked when application will be builded
    /// </param>
    /// <returns>
    /// Allows fluent usage by returning the same builder
    /// </returns>
    IXenaWebApplicationBuilder AddPostBuildAsyncAction(Func<IXenaWebApplication, Task> action);

    /// <summary>
    /// BuildAsync application
    /// </summary>
    /// <returns>
    /// Returns <see cref="IXenaWebApplication"/>
    /// </returns>
    Task<IXenaWebApplication> BuildAsync();
}