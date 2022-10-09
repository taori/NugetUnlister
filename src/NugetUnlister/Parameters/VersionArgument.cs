using System.CommandLine;

namespace NugetUnlister.Parameters;

public class VersionArgument : Argument<string>
{
	public VersionArgument() : base("packageVersion", "semantic version to act upon")
	{

	}
}