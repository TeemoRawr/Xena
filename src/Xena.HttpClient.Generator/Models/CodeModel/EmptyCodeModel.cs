using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Models.CodeModel;

public class EmptyCodeModel : BaseCodeModel
{
    public EmptyCodeModel(string name, OpenApiSchema schema) : base(name, schema)
    {
    }

    protected override CodeModelGenerationResult GenerateInternal(CodeModelGenerateOptions options)
    {
        return new CodeModelGenerationResult
        {
            Member = null
        };
    }
}