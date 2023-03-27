using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Models;

public class FloatCodeModel : BasicTypeCodeModel<float>
{
    public FloatCodeModel(string name, OpenApiSchema openApiSchema) : base(name, openApiSchema)
    {
    }
}