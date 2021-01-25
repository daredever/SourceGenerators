using Microsoft.CodeAnalysis;

namespace Generators
{
	public partial class AugmentingGenerator
	{
		public void Initialize(GeneratorInitializationContext context)
		{
			// Register a factory that can create our custom syntax receiver
			context.RegisterForSyntaxNotifications(() => new CustomSyntaxReceiver());
		}
	}
}