using System.Net;
using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Options;
using Xena.Discovery.Consul.Configuration;
using Xena.Discovery.Interfaces;
using Xena.Discovery.Models;

namespace Xena.Discovery.Consul;

internal class ConsulDiscoveryServicesService : IInitializeDiscoveryServicesService, IDiscoveryServicesService, IDisposable
{
    private IConsulClient? _consulClient;
    private readonly IServer _serverAddressesFeature;
    private readonly IOptions<ConsulDiscoveryServicesConfiguration> _consulOptions;
    private readonly List<Service> _services = new ();

    public bool Initialized { get; private set; }

    public ConsulDiscoveryServicesService(IServer serverAddressesFeature, IOptions<ConsulDiscoveryServicesConfiguration> consulOptions)
    {
        _serverAddressesFeature = serverAddressesFeature;
        _consulOptions = consulOptions;
    }

    public async Task InitializeAsync(CancellationToken stoppingToken)
    {
        var waitCounter = 0;

        while (!stoppingToken.IsCancellationRequested && !Initialized && waitCounter < 30)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            waitCounter++;
        }

        if (!Initialized)
        {
            throw new Exception("Cannot initialize Consul");
        }
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

    public async Task RefreshServicesAsync(CancellationToken stoppingToken)
    {
        var servicesResponse = await _consulClient!.Agent.Services(stoppingToken);
        var servicesFromConsul = servicesResponse.Response.Values;
        var services = servicesFromConsul.Select(s => new Service
        {
            Id = s.ID,
            Name = s.ID,
            Address = s.Address,
            Port = s.Port
        }).ToList();

        _services.Clear();
        _services.AddRange(services);
    }

    public void Dispose()
    {
        _consulClient?.Dispose();
    }

    internal async Task InitializeConsulAsync()
    {
        var addresses = _serverAddressesFeature.Features.Get<IServerAddressesFeature>();

        if (addresses is null)
        {
            throw new NullReferenceException();
        }

        var consulDiscoveryServicesConfiguration = _consulOptions.Value;

        _consulClient = new ConsulClient(configuration =>
        {
            configuration.Address = new Uri(consulDiscoveryServicesConfiguration.Host);
        });

        var preferredAddress = addresses.Addresses.First();

        var uri = new Uri(preferredAddress);

        var checkRegister = await _consulClient.Agent.CheckRegister(new AgentCheckRegistration
        {
            ID = consulDiscoveryServicesConfiguration.Id,
            Name = consulDiscoveryServicesConfiguration.Name,
            TTL = TimeSpan.FromSeconds(10)
        });

        if (checkRegister.StatusCode == HttpStatusCode.NotFound)
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
            Tags = new[] { consulDiscoveryServicesConfiguration.Name },
        });

        if (serviceRegister.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception(
                $"Error occurred while register service with name {consulDiscoveryServicesConfiguration.Name} exists in Consul.");
        }

        Initialized = true;
    }
}