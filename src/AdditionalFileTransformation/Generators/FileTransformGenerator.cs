using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Generators
{
	[Generator]
	public class FileTransformGenerator : ISourceGenerator
	{
		private static readonly DiagnosticDescriptor Warning = new(
			id: "FTGEN001",
			title: "Could find XML file",
			messageFormat: "Could find XML file '{0}'",
			category: "FileTransformGenerator",
			defaultSeverity: DiagnosticSeverity.Error,
			isEnabledByDefault: true);
		
		public void Initialize(GeneratorInitializationContext context)
		{
		}

		public void Execute(GeneratorExecutionContext context)
		{
			var recordsXml = context.AdditionalFiles.SingleOrDefault(a => Path.GetFileName(a.Path) == "Records.xml");
			if (recordsXml is null)
			{
				context.ReportDiagnostic(Diagnostic.Create(Warning, Location.None, "Records.xml"));
				return;
			}

			var records = XElement.Parse(recordsXml.GetText(context.CancellationToken).ToString());
			var names = records.Descendants("Record").Select(x => (string) x.Attribute("Name"));
			foreach (var name in names)
			{
				context.AddSource($"{name}.cs", SourceText.From($@"
namespace Records
{{
    public record {name}();
}}", Encoding.UTF8));
			}
		}
	}
}