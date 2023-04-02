using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Models.CodeModel;

public class IntCodeModel : BasicTypeCodeModel<int>
{
    public IntCodeModel(string name, OpenApiSchema openApiSchema) : base(name, openApiSchema)
    {
    }
}