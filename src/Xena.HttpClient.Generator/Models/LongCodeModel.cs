using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Models;

public class LongCodeModel : BasicTypeCodeModel<long>
{
    public LongCodeModel(string name, OpenApiSchema openApiSchema) : base(name, openApiSchema)
    {
    }
}