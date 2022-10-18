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

		this.SetHandler(async (package, pattern) =>
		{
			await ListHelper.ListPatternAsync(package, pattern, null);
		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.RegexArgument);
	}
}
