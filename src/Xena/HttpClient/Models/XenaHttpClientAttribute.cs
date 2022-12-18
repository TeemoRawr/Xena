namespace Xena.HttpClient.Models;

[AttributeUsage(AttributeTargets.Interface)]
public class XenaHttpClientAttribute : Attribute
{
    public string Name { get; set; }
}