using System.CommandLine;

namespace NugetUnlister.Parameters;

public class SourceServerOption : Option<string>
{
	public SourceServerOption() : base(
		"--source",
		() => "https://api.nuget.org/v3-flatcontainer/",
		"Repository source, e.g. https://api.nuget.org/v3-flatcontainer/ (see https://docs.microsoft.com/de-de/dotnet/core/tools/dotnet-nuget-delete?tabs=netcore2x)")
	{
	}
}
