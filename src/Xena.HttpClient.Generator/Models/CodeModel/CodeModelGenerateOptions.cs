namespace Xena.HttpClient.Generator.Models.CodeModel;

public record CodeModelGenerateOptions
{
    public string Prefix { get; init; } = null!;
    public bool IsRequired { get; init; }
}