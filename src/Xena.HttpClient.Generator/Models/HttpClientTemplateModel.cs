namespace Xena.HttpClient.Generator.Models;

public class HttpClientTemplateModel
{
    public string HttpClientName { get; init; } = null!;
    public List<ClientModel> Schemas { get; init; } = null!;
}