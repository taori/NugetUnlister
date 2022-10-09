using System.CommandLine;

namespace NugetUnlister.Parameters;

public class PackageNameArgument : Argument<string>
{
	public PackageNameArgument() : base("packageName", "package identifier of the nuget package")
	{

	}
}