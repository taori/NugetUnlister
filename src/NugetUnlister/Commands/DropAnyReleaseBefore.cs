using System.CommandLine;
using NugetUnlister.Helpers;
using NugetUnlister.Parameters;

namespace NugetUnlister.Commands;

public class DropAnyReleaseBefore : Command
{
	public DropAnyReleaseBefore() : base("AnyReleaseBefore", "unlists prerelease + release versions prior to given version")
	{
		AddAlias("any");
		AddAlias("anyreleasebefore");
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddArgument(ApplicationParameters.VersionArgument);
		AddArgument(ApplicationParameters.ApiKeyArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (package, version, apiKey, src) =>
		{
			await DropHelper.DropBefore(package, version, apiKey, src, pre: true);
			await DropHelper.DropBefore(package, version, apiKey, src, pre: false);

		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.VersionArgument, ApplicationParameters.ApiKeyArgument, ApplicationParameters.SourceServerOption);
	}
}
