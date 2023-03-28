using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Models;

public class BoolCodeModel : BasicTypeCodeModel<DateTime>
{
    public BoolCodeModel(string name, OpenApiSchema schema) : base(name, schema)
    {
    }
}