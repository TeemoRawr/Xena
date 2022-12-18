namespace Xena.Discovery.Models;

public class Service
{
    private readonly List<string> _tags;

    public Service(string id, string name, string address, int port, List<string> tags)
    {
        Id = id;
        Name = name;
        Address = address;
        Port = port;

        _tags = tags;
        _tags.Add(id);
    }

    public string Id { get; }
    public string Name { get; }
    public string Address { get; }
    public int Port { get; }
    public IReadOnlyList<string> Tags => _tags;
}