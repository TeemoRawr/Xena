namespace Xena.Discovery.Models;

public class Service
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int Port { get; set; }
}