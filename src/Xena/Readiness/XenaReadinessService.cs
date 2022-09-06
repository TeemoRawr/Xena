using Microsoft.Extensions.Options;
using Xena.Readiness.Interfaces;
using Xena.Readiness.Models;

namespace Xena.Readiness;

internal class XenaReadinessService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<XenaReadinessService> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IOptions<XenaReadinessOptions> _xenaReadinessConfiguration;
    
    public XenaReadinessService(
        IServiceScopeFactory serviceScopeFactory, 
        ILogger<XenaReadinessService> logger, 
        IHostApplicationLifetime hostApplicationLifetime, 
        IOptions<XenaReadinessOptions> xenaReadinessConfiguration)
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

        if (behavior.HasFlag(XenaReadinessBehavior.LogWarning))
        {
            _logger.LogWarning(message);
        }

        if (behavior.HasFlag(XenaReadinessBehavior.LogError))
        {
            _logger.LogError(message);
        }

        if (behavior.HasFlag(XenaReadinessBehavior.LogCritical))
        {
            _logger.LogCritical(message);
        }

        if (behavior.HasFlag(XenaReadinessBehavior.TerminateApplication))
        {
            _logger.LogCritical($"Application is going to terminate cause readiness {serviceName} throw status {status}");
            _hostApplicationLifetime.StopApplication();
        }
    }
}