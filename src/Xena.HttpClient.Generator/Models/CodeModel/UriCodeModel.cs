using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Models.CodeModel;

public class UriCodeModel : BasicTypeCodeModel<Uri>
{
    public UriCodeModel(string name, OpenApiSchema schema) : base(name, schema)
    {
    }
}