using System.CommandLine;
using NugetUnlister.Helpers;
using NugetUnlister.Parameters;

namespace NugetUnlister.Commands;

public class ListReleaseBefore : Command
{
	public ListReleaseBefore() : base("ReleaseBefore", "unlists release versions prior to given version")
	{
		AddAlias("rel");
		AddAlias("releasebefore");
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddArgument(ApplicationParameters.VersionArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (packageName, version, packageSource) =>
		{
			await ListHelper.ListAsync(packageName, version, false, packageSource);
		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.VersionArgument, ApplicationParameters.SourceServerOption);
	}
}
