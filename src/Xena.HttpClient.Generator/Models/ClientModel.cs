namespace Xena.HttpClient.Generator.Models;

public class ClientModel
{
    public string Name { get; init; } = null!;
    public IReadOnlyList<ClientModelProperty> Properties { get; init; } = null!;
}

public class ClientModelProperty
{
    public string Name { get; init; } = null!;

    public string PropertyType { get; set; }
    public string FinalPropertyType => PropertyType + (Nullable ? "?" : string.Empty);

    
    public bool Nullable { get; set; }
    public bool IsCollection { get; set; }
}