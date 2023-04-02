using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Models.CodeModel;

public class EmptyCodeModel : BaseCodeModel
{
    public EmptyCodeModel(string name, OpenApiSchema schema) : base(name, schema)
    {
    }

    protected override CodeModelGenerationResult<MemberDeclarationSyntax> GenerateInternal(CodeModelGenerateOptions options)
    {
        return new CodeModelGenerationResult<MemberDeclarationSyntax>
        {
            Member = null
        };
    }
}