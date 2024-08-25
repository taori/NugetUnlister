using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using NugetUnlister.Interfaces;
using NugetUnlister.Parameters;
using NugetUnlister.Scopes;

namespace NugetUnlister.Commands;

public class DropAnyReleaseBefore : Command
{
	public DropAnyReleaseBefore() : base("AnyReleaseBefore", "unlists prerelease + release versions prior to given version")
	{
		AddAlias("any");
		AddAlias("anyreleasebefore");
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddArgument(ApplicationParameters.VersionArgument);
		AddArgument(ApplicationParameters.ApiKeyArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (package, version, apiKey, src, verbosity) =>
		{
			var api = ServiceScope.Current?.ServiceProvider.GetRequiredService<IApiCalls>() ?? throw new CliUnavailableException();
			await api.DropBeforeAsync(verbosity, package, version, apiKey, src, null);

		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.VersionArgument, ApplicationParameters.ApiKeyArgument, ApplicationParameters.SourceServerOption, ApplicationParameters.VerbosityOption);
	}
}
