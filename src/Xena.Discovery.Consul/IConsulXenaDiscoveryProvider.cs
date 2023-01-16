using Xena.Discovery.Interfaces;

namespace Xena.Discovery.Consul;

internal interface IConsulXenaDiscoveryProvider : IXenaDiscoveryProvider, IDisposable
{
    Task InitializeConsulAsync();
    Task DeactivateAsync();
}