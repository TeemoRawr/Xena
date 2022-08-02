namespace Xena.Discovery.Models;

public class Service
{
    private readonly List<string> _tags = new();

    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int Port { get; set; }

    public IReadOnlyList<string> Tags
    {
        get => _tags;
        init
        {
            _tags.AddRange(value);
        }
    }

    public void AddTags(params string[] tags)
    {
        _tags.AddRange(tags);
    }
}