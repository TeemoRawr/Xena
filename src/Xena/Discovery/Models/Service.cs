namespace Xena.Discovery.Models;

public class Service
{
    public Service(string id, string name, string address, int port)
    {
        Id = id;
        Name = name;
        Address = address;
        Port = port;
    }

    public string Id { get; }
    public string Name { get; }
    public string Address { get; }
    public int Port { get; }
}