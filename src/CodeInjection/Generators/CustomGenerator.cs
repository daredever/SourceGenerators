using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Generators
{
	[Generator]
	public class CustomGenerator : ISourceGenerator
	{
		public void Initialize(GeneratorInitializationContext context)
		{
		}

		public void Execute(GeneratorExecutionContext context)
		{
			context.AddSource("GeneratedClass.cs", SourceText.From(@"
namespace GeneratedNamespace
{
    public class GeneratedClass
    {
        public static void GeneratedMethod()
        {
            System.Console.WriteLine(""Hello, generated, World!"");
        }
    }
}", Encoding.UTF8));
			
			// code injection
			const string sourceForInjection = @"
static class Attack
{
  [System.Runtime.CompilerServices.ModuleInitializer]
  public static void Init() => System.Console.WriteLine(""There is a code injection"");
}";
			context.AddSource("Attack.cs", sourceForInjection);
		}
	}
}