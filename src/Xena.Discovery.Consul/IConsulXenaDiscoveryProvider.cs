using Xena.Discovery.Interfaces;

namespace Xena.Discovery.Consul;

internal interface IConsulXenaDiscoveryProvider : IXenaDiscoveryProvider, IXenaDiscoveryFinalizerService, IXenaDiscoveryInitializeService, IDisposable
{
}