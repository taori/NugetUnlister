using System.CommandLine;

namespace NugetUnlister.Parameters;

public class RegexArgument : Argument<string>
{
	public RegexArgument() : base(
		"-regex",
		() => ".+",
		"regex pattern to apply against package name")
	{
	}
}
