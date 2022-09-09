using System.Net;
using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xena.Discovery.Consul.Configuration;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;

namespace Xena.Discovery.Consul;

internal class ConsulXenaDiscoveryProvider : IXenaDiscoveryProvider, IDisposable
{
    private bool _isInitialized;
    private IConsulClient? _consulClient;
    private readonly IServer _serverAddressesFeature;
    private readonly IOptions<ConsulXenaDiscoveryServicesConfiguration> _consulOptions;
    private readonly List<Service> _services = new ();
    private readonly ILogger<ConsulXenaDiscoveryProvider> _logger;

    public ConsulXenaDiscoveryProvider(
        IServer serverAddressesFeature, 
        IOptions<ConsulXenaDiscoveryServicesConfiguration> consulOptions, 
        ILogger<ConsulXenaDiscoveryProvider> logger)
    {
        _serverAddressesFeature = serverAddressesFeature;
        _consulOptions = consulOptions;
        _logger = logger;
    }

    public Task DeactivateAsync()
    {
        var consulDiscoveryServicesConfiguration = _consulOptions.Value;

        return _consulClient!.Agent.ServiceDeregister(consulDiscoveryServicesConfiguration.Id);
    }

    public Task AddServiceAsync(Service service)
    {
        _services.Add(service);
        return Task.CompletedTask;
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
    }

    public void Dispose()
    {
        _consulClient?.Dispose();
    }

    public async Task InitializeConsulAsync()
    {
        var addresses = _serverAddressesFeature.Features.Get<IServerAddressesFeature>();

        if (addresses is null)
        {
            throw new NullReferenceException();
        }

        var consulDiscoveryServicesConfiguration = _consulOptions.Value;

        if (consulDiscoveryServicesConfiguration is null)
        {
            throw new ArgumentNullException(
                nameof(consulDiscoveryServicesConfiguration),
                "Missing configuration for Consul provider");
        }

        _consulClient = new ConsulClient(configuration =>
        {
            configuration.Address = new Uri(consulDiscoveryServicesConfiguration.Host);
        });

        var preferredAddress = addresses.Addresses.First();

        var uri = new Uri(preferredAddress);

#if DEBUG
        await _consulClient.Agent.ServiceDeregister(consulDiscoveryServicesConfiguration.Id);
#endif

        var checkRegister = await 
            _consulClient.Health.Service(consulDiscoveryServicesConfiguration.Id);

        if (checkRegister.Response.Any())
        {
            throw new Exception(
                $"Service with name {consulDiscoveryServicesConfiguration.Name} exists in Consul. Please check name of application");
        }

        var serviceRegister = await _consulClient.Agent.ServiceRegister(new AgentServiceRegistration
        {
            ID = consulDiscoveryServicesConfiguration.Id,
            Name = consulDiscoveryServicesConfiguration.Name,
            Address = $"{uri.Scheme}://{uri.Host}:{uri.Port}",
            Port = uri.Port,
            Tags = new[] { consulDiscoveryServicesConfiguration.Name },
            Checks = new []
            {
                new AgentServiceCheck
                {
                    Interval = TimeSpan.FromSeconds(3),
                    HTTP = $"{uri.Scheme}://{uri.Host}:{uri.Port}/xena-health-check",
                    // ID = "xena-health-check",
                    Name = "Xena Health Check",
                    Method = "GET",
                    TLSSkipVerify = true,
                    Timeout = TimeSpan.FromSeconds(5)
                }
            }
        });

        if (serviceRegister.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception(
                $"Error occurred while register service with name {consulDiscoveryServicesConfiguration.Name} exists in Consul.");
        }

        _isInitialized = true;
    }
}