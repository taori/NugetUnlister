using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using NugetUnlister.Interfaces;
using NugetUnlister.Parameters;
using NugetUnlister.Scopes;

namespace NugetUnlister.Commands;

public class DropLike : Command
{
	public DropLike() : base("like", "unlists release versions prior to given version")
	{
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddArgument(ApplicationParameters.RegexArgument);
		AddArgument(ApplicationParameters.ApiKeyArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (package, pattern, apiKey, src, verbosity) =>
		{
			var api = ServiceScope.Current?.ServiceProvider.GetRequiredService<IApiCalls>() ?? throw new CliUnavailableException();
			await api.DropLikeAsync(verbosity, package, pattern, apiKey, src, prerelease: null);

		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.RegexArgument, ApplicationParameters.ApiKeyArgument, ApplicationParameters.SourceServerOption, ApplicationParameters.VerbosityOption);
	}
}
