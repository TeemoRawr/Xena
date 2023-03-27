using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Models;

public class DoubleCodeModel : BasicTypeCodeModel<double>
{
    public DoubleCodeModel(string name, OpenApiSchema openApiSchema) : base(name, openApiSchema)
    {
    }
}