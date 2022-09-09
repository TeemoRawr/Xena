namespace Xena.HttpClient.Models;

public interface IXenaHttpClient
{
}

[AttributeUsage(AttributeTargets.Class)]
public class XenaHttpClientAttribute : Attribute
{
    public string Name { get; set; }
}