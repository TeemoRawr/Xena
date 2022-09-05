using Microsoft.Extensions.Options;
using Xena.HealthCheck;
using Xena.Startup;

namespace Xena.Readiness;

public static class XenaReadinessServiceExtensions
{
    public static IXenaWebApplicationBuilder AddReadiness(
        this IXenaWebApplicationBuilder webApplicationBuilder,
        Action<IXenaReadinessConfigurator>? configurationAction = null)
    {
        var xenaReadinessConfiguration = new XenaReadinessConfigurator(webApplicationBuilder);
        configurationAction?.Invoke(xenaReadinessConfiguration);

        webApplicationBuilder.WebApplicationBuilder.Services.AddTransient<XenaReadinessService>();

        webApplicationBuilder.AddPostBuildAsyncAction(async p =>
        {
            var xenaReadinessService = p.Services.GetRequiredService<XenaReadinessService>();
            await xenaReadinessService.CheckReadiness();
        });

        return webApplicationBuilder;
    }
}

public interface IXenaReadinessConfigurator
{
    IXenaReadinessConfigurator EnableAutoDiscoveryReadiness();
}

internal class XenaReadinessConfigurator : IXenaReadinessConfigurator
{
    private readonly IXenaWebApplicationBuilder _xenaWebApplicationBuilder;

    public XenaReadinessConfigurator(IXenaWebApplicationBuilder xenaWebApplicationBuilder)
    {
        _xenaWebApplicationBuilder = xenaWebApplicationBuilder;
    }

    public IXenaReadinessConfigurator EnableAutoDiscoveryReadiness()
    {
        var xenaReadinessTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(p => p.GetTypes())
            .Where(t => typeof(IXenaReadiness).IsAssignableTo(t))
            .ToList();

        foreach (var xenaHealthCheckType in xenaReadinessTypes)
        {
            _xenaWebApplicationBuilder.WebApplicationBuilder.Services.AddScoped(
                typeof(IXenaHealthCheck),
                xenaHealthCheckType);
        }

        return this;
    }
}

internal class XenaReadinessService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<XenaReadinessService> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IOptions<XenaReadinessConfiguration> _xenaReadinessConfiguration;
    
    public XenaReadinessService(
        IServiceScopeFactory serviceScopeFactory, 
        ILogger<XenaReadinessService> logger, 
        IHostApplicationLifetime hostApplicationLifetime, 
        IOptions<XenaReadinessConfiguration> xenaReadinessConfiguration)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _xenaReadinessConfiguration = xenaReadinessConfiguration;
    }

    public async Task CheckReadiness()
    {
        var xenaReadinessConfiguration = _xenaReadinessConfiguration.Value;

        using var serviceScope = _serviceScopeFactory.CreateScope();

        var xenaReadinesses = serviceScope.ServiceProvider.GetServices<IXenaReadiness>().ToList();

        foreach (var xenaReadiness in xenaReadinesses)
        {
            var status = await xenaReadiness.CheckAsync(serviceScope.ServiceProvider);

            if (status == XenaReadinessStatus.Successful)
            {
                var behaviorOnSuccessful = xenaReadinessConfiguration.BehaviorOnSuccessful;
                ExecuteBehavior(xenaReadiness, behaviorOnSuccessful, XenaReadinessStatus.Successful);
                continue;
            }

            if (status == XenaReadinessStatus.Warning)
            {
                var behaviorOnWarning = xenaReadinessConfiguration.BehaviorOnWarning;
                ExecuteBehavior(xenaReadiness, behaviorOnWarning, XenaReadinessStatus.Warning);
                continue;
            }

            if (status == XenaReadinessStatus.Error)
            {
                var behaviorOnError = xenaReadinessConfiguration.BehaviorOnError;
                ExecuteBehavior(xenaReadiness, behaviorOnError, XenaReadinessStatus.Error);
                continue;
            }

            throw new ArgumentOutOfRangeException(nameof(status));
        }
    }

    private void ExecuteBehavior(IXenaReadiness xenaReadiness, XenaReadinessBehavior behavior, XenaReadinessStatus status)
    {
        var serviceName = xenaReadiness.GetType().FullName;
        var message = $"{serviceName} return {status} status";

        if (behavior == XenaReadinessBehavior.Nothing)
        {
            return;
        }

        if (behavior.HasFlag(XenaReadinessBehavior.LogInformation))
        {
            _logger.LogInformation(message);
        }
        else if (behavior.HasFlag(XenaReadinessBehavior.LogWarning))
        {
            _logger.LogWarning(message);
        }
        else if (behavior.HasFlag(XenaReadinessBehavior.LogError))
        {
            _logger.LogError(message);
        }
        else if (behavior.HasFlag(XenaReadinessBehavior.LogCritical))
        {
            _logger.LogCritical(message);
        }
        else if (behavior.HasFlag(XenaReadinessBehavior.TerminateApplication))
        {
            _logger.LogCritical($"Application is going to terminate cause readiness {serviceName} throw status {status}");
            _hostApplicationLifetime.StopApplication();
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(behavior), behavior, null);
        }
    }
}

public class XenaReadinessConfiguration
{
    public XenaReadinessBehavior BehaviorOnSuccessful { get; set; }
    public XenaReadinessBehavior BehaviorOnWarning { get; set; }
    public XenaReadinessBehavior BehaviorOnError { get; set; }
}

public enum XenaReadinessBehavior : short
{
    Nothing = 0,
    LogInformation = 1,
    LogWarning = 2,
    LogError = 4,
    LogCritical = 8,
    TerminateApplication = 16
}

public enum XenaReadinessStatus : short
{
    Successful,
    Warning,
    Error
}

internal interface IXenaReadiness
{
    Task<XenaReadinessStatus> CheckAsync(IServiceProvider serviceScopeServiceProvider);
}