using System.CommandLine;

namespace NugetUnlister.Parameters;

public class SourceServerOption : Option<string>
{
	public SourceServerOption() : base(
		"--source",
		() => "https://api.nuget.org/v3/index.json",
		"Repository source, e.g. https://api.nuget.org/v3/index.json (see https://docs.microsoft.com/de-de/dotnet/core/tools/dotnet-nuget-delete?tabs=netcore2x)")
	{
	}
}
