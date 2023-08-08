using System.Net;
using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Xena.Discovery.Consul.Configuration;
using Xena.Discovery.Models;
using Policy = Polly.Policy;

namespace Xena.Discovery.Consul;

internal class ConsulXenaDiscoveryProvider : IConsulXenaDiscoveryProvider
{
    private bool _isInitialized;
    private readonly IServer _serverAddressesFeature;
    private readonly IOptions<ConsulXenaDiscoveryServicesConfiguration> _consulOptions;
    private readonly List<Service> _services = new();
    private readonly ILogger<ConsulXenaDiscoveryProvider> _logger;
    private readonly IConsulClient _consulClient;

    public ConsulXenaDiscoveryProvider(
        IServer serverAddressesFeature,
        IOptions<ConsulXenaDiscoveryServicesConfiguration> consulOptions,
        ILogger<ConsulXenaDiscoveryProvider> logger,
        IConsulClient consulClient)
    {
        _serverAddressesFeature = serverAddressesFeature;
        _consulOptions = consulOptions;
        _logger = logger;
        _consulClient = consulClient;
    }

    public Task AddServiceAsync(Service service)
    {
        _services.Add(service);
        return Task.CompletedTask;
    }

    public Service? GetService(string id)
    {
        var service = _services.SingleOrDefault(s => s.Id == id);
        return service;
    }

    public Task<Service?> GetServiceAsync(string id)
    {
        var service = _services.SingleOrDefault(s => s.Id == id);
        return Task.FromResult(service);
    }

    public Task<IReadOnlyList<Service>> FindByTagAsync(string tag)
    {
        var services = _services.Where(s => s.Tags.Contains(tag)).ToList();
        return Task.FromResult<IReadOnlyList<Service>>(services);
    }

    public async Task RefreshServicesAsync(CancellationToken stoppingToken)
    {
        if (!_isInitialized)
        {
            _logger.LogInformation("Consul is not initialized yet");
            return;
        }

        _logger.LogInformation("Start refreshing services from Consul");

        var consulDiscoveryServicesConfiguration = _consulOptions.Value;

        await _consulClient!.Agent.PassTTL(consulDiscoveryServicesConfiguration.Id, "Ok", stoppingToken);

        var servicesResponse = await _consulClient!.Agent.Services(stoppingToken);
        var servicesFromConsul = servicesResponse.Response.Values;
        var services = servicesFromConsul
            .Select(s => new Service(
                s.ID,
                s.ID,
                s.Address,
                s.Port,
                s.Tags.ToList()))
            .ToList();

        _services.Clear();
        _services.AddRange(services);

        _logger.LogInformation("Refreshing services from Consul done");
    }

    public Task FinalizeAsync()
    {
        var consulDiscoveryServicesConfiguration = _consulOptions.Value;

        return _consulClient!.Agent.ServiceDeregister(consulDiscoveryServicesConfiguration.Id);
    }

    public async Task InitializeAsync()
    {
        var preferredAddress = Policy.HandleResult<IServerAddressesFeature>(addresses => 
                addresses is null || !addresses.Addresses.Any())
            .WaitAndRetry(5, i => TimeSpan.FromSeconds(1))
            .Execute(() => _serverAddressesFeature.Features.Get<IServerAddressesFeature>());

        var uri = new Uri(preferredAddress.Addresses.First());
        var consulDiscoveryServicesConfiguration = _consulOptions.Value;

#if DEBUG
        _logger.LogDebug($"Unregister service {consulDiscoveryServicesConfiguration.Id} from Consul");
        await _consulClient.Agent.ServiceDeregister(consulDiscoveryServicesConfiguration.Id);
#endif

        var checkRegister = await _consulClient.Health.Service(consulDiscoveryServicesConfiguration.Id);

        if (checkRegister.Response.Any())
        {
            throw new Exception(
                $"Service with name {consulDiscoveryServicesConfiguration.Name} exists in Consul. Please check name of application");
        }

        var serviceRegister = await _consulClient.Agent.ServiceRegister(new AgentServiceRegistration
        {
            ID = consulDiscoveryServicesConfiguration.Id,
            Name = consulDiscoveryServicesConfiguration.Name,
            Address = $"{uri.Scheme}://{uri.Host}",
            Port = uri.Port,
            Tags = new[] { consulDiscoveryServicesConfiguration.Name }
        });

        if (serviceRegister.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"Error occurred while register service with name {consulDiscoveryServicesConfiguration.Name} exists in Consul.");
        }

        _isInitialized = true;
        _logger.LogDebug("Consul Xena discovery initialized");
        await RefreshServicesAsync(CancellationToken.None);
    }

    public void Dispose()
    {
        _consulClient?.Dispose();
    }
}