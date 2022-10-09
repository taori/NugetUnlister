using System.CommandLine;
using NugetUnlister.Helpers;
using NugetUnlister.Parameters;

namespace NugetUnlister.Commands;

public class DropPrereleaseBefore : Command
{
	public DropPrereleaseBefore() : base("PrereleaseBefore", "unlists prerelease versions prior to given version")
	{
		AddAlias("pre");
		AddAlias("prereleasebefore");
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddArgument(ApplicationParameters.VersionArgument);
		AddArgument(ApplicationParameters.ApiKeyArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (package, version, apiKey, src) =>
		{
			await DropHelper.DropBefore(package, version, apiKey, src, pre: true);

		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.VersionArgument, ApplicationParameters.ApiKeyArgument, ApplicationParameters.SourceServerOption);
	}
}
