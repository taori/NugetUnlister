using System.CommandLine;
using NugetUnlister.Helpers;
using NugetUnlister.Parameters;

namespace NugetUnlister.Commands;

public class ListPrereleaseBefore : Command
{
	public ListPrereleaseBefore() : base("PrereleaseBefore", "unlists prerelease versions prior to given version")
	{
		AddAlias("pre");
		AddAlias("prereleasebefore");
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddArgument(ApplicationParameters.VersionArgument);

		this.SetHandler(async (package, version) =>
		{
			await ListHelper.ListAsync(package, version, true);
		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.VersionArgument);
	}
}
