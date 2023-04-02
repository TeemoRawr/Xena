using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Models.CodeModel;

public class StringCodeModel : BasicTypeCodeModel<string>
{
    public StringCodeModel(string name, OpenApiSchema openApiSchema) : base(name, openApiSchema)
    {
    }
}