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

		this.SetHandler(async (package, version) =>
		{
			await ListHelper.ListAsync(package, version, false);
		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.VersionArgument);
	}
}
