using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Generators
{
	public partial class AugmentingGenerator
	{
		public void Execute(GeneratorExecutionContext context)
		{
			// the generator infrastructure will create a receiver and populate it
			// we can retrieve the populated instance via the context
			var directoryOfCountriesClass = ((CustomSyntaxReceiver) context.SyntaxReceiver).ClassToAugment;
			if (directoryOfCountriesClass is null)
			{
				return;
			}

			// get data from project
			var xmlData = context.AdditionalFiles
				.SingleOrDefault(a => Path.GetFileName(a.Path) == "Countries.xml")
				?.GetText(context.CancellationToken)
				?.ToString();
			if (xmlData is null)
			{
				ReportDiagnostic(context);
				return;
			}

			AddSource(context, xmlData);
		}

		private static void AddSource(GeneratorExecutionContext context, string xmlData)
		{
			var generatedSource = GenerateSource(xmlData);
			context.AddSource("DirectoryOfCountries.Generated.cs", SourceText.From(generatedSource, Encoding.UTF8));

			var countrySource = "namespace Countries{ public record Country (string Name, string Code); }";
			context.AddSource("Country.cs", SourceText.From(countrySource, Encoding.UTF8));
		}

		private static void ReportDiagnostic(GeneratorExecutionContext context)
		{
			var error = new DiagnosticDescriptor(
				id: "AGEN001",
				title: "Could find XML file",
				messageFormat: "Could find XML file '{0}'",
				category: "FileTransformGenerator",
				defaultSeverity: DiagnosticSeverity.Error,
				isEnabledByDefault: true);

			context.ReportDiagnostic(Diagnostic.Create(error, Location.None, "Countries.xml"));
		}

		private static string GenerateSource(string countriesXml)
		{
			var countries = XElement.Parse(countriesXml)
				.Descendants("country")
				.Select(x => new
				{
					Name = x.Element("name")?.Value,
					Code = x.Element("alpha2")?.Value,
					Location = x.Element("location")?.Value,
				});

			var sb = new StringBuilder();
			sb.AppendLine("using System.Collections.Generic; namespace Countries {");
			sb.AppendLine("public partial class DirectoryOfCountries {");
			sb.AppendLine("private static List<Country> _europe = new List<Country> {");
			foreach (var country in countries.Where(c => c.Location == "Европа"))
			{
				sb.Append($"new Country (\"{country.Name}\", \"{country.Code}\"),");
			}

			sb.AppendLine("};");
			sb.AppendLine("public static partial IReadOnlyList<Country> Europe() => _europe; }}");

			return sb.ToString();
		}
	}
}