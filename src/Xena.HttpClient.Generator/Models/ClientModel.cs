namespace Xena.HttpClient.Generator.Models;

public class ClientModel
{
    public string Name { get; init; } = null!;
    public IReadOnlyList<ClientModelProperty> Properties { get; init; } = null!;
}
