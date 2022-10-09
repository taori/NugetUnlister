using System.CommandLine;
using NugetUnlister.Helpers;
using NugetUnlister.Parameters;

namespace NugetUnlister.Commands;

public class DropReleaseBefore : Command
{
	public DropReleaseBefore() : base("ReleaseBefore", "unlists release versions prior to given version")
	{
		AddAlias("rel");
		AddAlias("releasebefore");
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddArgument(ApplicationParameters.VersionArgument);
		AddArgument(ApplicationParameters.ApiKeyArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (package, version, apiKey, src) =>
		{
			await DropHelper.DropBefore(package, version, apiKey, src, pre: false);

		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.VersionArgument, ApplicationParameters.ApiKeyArgument, ApplicationParameters.SourceServerOption);
	}
}
