using System;
using System.Linq;
using System.Xml;
using Microsoft.CodeAnalysis;

namespace Generators
{
	[Generator]
	public class MyXmlGenerator : ISourceGenerator
	{
		private static readonly DiagnosticDescriptor InvalidXmlWarning = new(
			id: "MYXMLGEN001",
			title: "Couldn't parse XML file",
			messageFormat: "Couldn't parse XML file '{0}'",
			category: "MyXmlGenerator",
			defaultSeverity: DiagnosticSeverity.Warning,
			isEnabledByDefault: true);

		public void Initialize(GeneratorInitializationContext context)
		{
		}

		public void Execute(GeneratorExecutionContext context)
		{
			var xmlFiles = context.AdditionalFiles
				.Where(at => at.Path.EndsWith(".xml", StringComparison.OrdinalIgnoreCase));
			foreach (var xmlFile in xmlFiles)
			{
				var text = xmlFile.GetText(context.CancellationToken).ToString();
				var xmlDoc = new XmlDocument();
				try
				{
					xmlDoc.LoadXml(text);
				}
				catch (XmlException)
				{
					// issue warning MYXMLGEN001: Couldn't parse XML file '<path>'
					context.ReportDiagnostic(Diagnostic.Create(InvalidXmlWarning, Location.None, xmlFile.Path));
					continue;
				}

				// continue generation...
			}
		}
	}
}