using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators
{
	[Generator]
	public partial class AugmentingGenerator : ISourceGenerator
	{
		private class CustomSyntaxReceiver : ISyntaxReceiver
		{
			public ClassDeclarationSyntax ClassToAugment { get; private set; }

			public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
			{
				// Business logic to decide what we're interested in goes here
				if (syntaxNode is ClassDeclarationSyntax cds && cds.Identifier.ValueText == "DirectoryOfCountries")
				{
					ClassToAugment = cds;
				}
			}
		}
	}
}