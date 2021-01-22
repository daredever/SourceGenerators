using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Generators
{
	[Generator]
	public class AugmentingGenerator : ISourceGenerator
	{
		public void Initialize(GeneratorInitializationContext context)
		{
			// Register a factory that can create our custom syntax receiver
			context.RegisterForSyntaxNotifications(() => new MySyntaxReceiver());
		}

		public void Execute(GeneratorExecutionContext context)
		{
			// the generator infrastructure will create a receiver and populate it
			// we can retrieve the populated instance via the context
			var syntaxReceiver = (MySyntaxReceiver)context.SyntaxReceiver;

			// get the recorded user class
			var userClass = syntaxReceiver.ClassToAugment;
			if (userClass is null)
			{
				// if we didn't find the user class, there is nothing to do
				return;
			}

			// add the generated implementation to the compilation
			var sourceText = SourceText.From($@"
namespace ConsoleApp
{{
	public partial class {userClass.Identifier}
	{{
	    private void GeneratedMethod()
	    {{
	        // generated code
	    }}

	    public partial void PartialUserMethod(string message) => System.Console.WriteLine(message);

	    public partial string PartialUserMethodWithResult(string message) => message;
	}}
}}", Encoding.UTF8);
			context.AddSource("UserClass.Generated.cs", sourceText);
		}

		class MySyntaxReceiver : ISyntaxReceiver
		{
			public ClassDeclarationSyntax ClassToAugment { get; private set; }

			public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
			{
				// Business logic to decide what we're interested in goes here
				if (syntaxNode is ClassDeclarationSyntax cds &&
				    cds.Identifier.ValueText == "UserClass")
				{
					ClassToAugment = cds;
				}
			}
		}
	}
}