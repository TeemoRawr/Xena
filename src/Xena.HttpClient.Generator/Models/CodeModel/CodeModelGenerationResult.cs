using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Xena.HttpClient.Generator.Models.CodeModel;

public record CodeModelGenerationResult<TResult> where TResult : CSharpSyntaxNode
{
    public MemberDeclarationSyntax? Member { get; init; } = null!;

    public IReadOnlyList<MemberDeclarationSyntax> ExtraObjectMembers { get; init; } =
        new List<MemberDeclarationSyntax>(0);
}