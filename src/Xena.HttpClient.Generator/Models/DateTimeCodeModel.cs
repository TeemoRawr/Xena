using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Models;

public class DateTimeCodeModel : BasicTypeCodeModel<DateTime>
{
    public DateTimeCodeModel(string name, OpenApiSchema schema) : base(name, schema)
    {
    }
}