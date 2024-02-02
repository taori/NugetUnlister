using System.CommandLine;
using NugetUnlister.Helpers;
using NugetUnlister.Parameters;

namespace NugetUnlister.Commands;

public class ListLike : Command
{
	public ListLike() : base("like", "unlists release versions prior to given version")
	{
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddArgument(ApplicationParameters.RegexArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (packageName, pattern, packageSource) =>
		{
			await ListHelper.ListPatternAsync(packageName, pattern, null, packageSource);
		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.RegexArgument, ApplicationParameters.SourceServerOption);
	}
}
