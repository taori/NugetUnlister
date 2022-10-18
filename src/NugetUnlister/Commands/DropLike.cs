using System.CommandLine;
using NugetUnlister.Helpers;
using NugetUnlister.Parameters;

namespace NugetUnlister.Commands;

public class DropLike : Command
{
	public DropLike() : base("like", "unlists release versions prior to given version")
	{
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddArgument(ApplicationParameters.RegexArgument);
		AddArgument(ApplicationParameters.ApiKeyArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (package, pattern, apiKey, src) =>
		{
			await DropHelper.DropLike(package, pattern, apiKey, src, pre: null);

		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.RegexArgument, ApplicationParameters.ApiKeyArgument, ApplicationParameters.SourceServerOption);
	}
}
