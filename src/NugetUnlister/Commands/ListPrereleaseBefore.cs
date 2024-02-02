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
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (packageName, version, packageSource) =>
		{
			await ListHelper.ListAsync(packageName, version, true, packageSource);
		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.VersionArgument, ApplicationParameters.SourceServerOption);
	}
}
