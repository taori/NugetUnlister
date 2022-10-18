using System.CommandLine;

namespace NugetUnlister.Parameters;

public class SourceServerOption : Option<string>
{
	public SourceServerOption() : base(
		"--source",
		() => "https://www.nuget.org",
		"Repository source, e.g. https://www.nuget.org, https://www.nuget.org/api/v3, https://www.nuget.org/api/v2/package (see https://docs.microsoft.com/de-de/dotnet/core/tools/dotnet-nuget-delete?tabs=netcore2x)")
	{
	}
}
