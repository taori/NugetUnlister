using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using NugetUnlister.Interfaces;
using NugetUnlister.Parameters;
using NugetUnlister.Scopes;

namespace NugetUnlister.Commands;

public class DropReleaseBefore : Command
{
	public DropReleaseBefore() : base("ReleaseBefore", "unlists release versions prior to given version")
	{
		AddAlias("rel");
		AddAlias("releasebefore");
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddArgument(ApplicationParameters.VersionArgument);
		AddArgument(ApplicationParameters.ApiKeyArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (package, version, apiKey, src, verbosity) =>
		{
			var api = ServiceScope.Current?.ServiceProvider.GetRequiredService<IApiCalls>() ?? throw new CliUnavailableException();
			await api.DropBeforeAsync(verbosity, package, version, apiKey, src, false);

		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.VersionArgument, ApplicationParameters.ApiKeyArgument, ApplicationParameters.SourceServerOption, ApplicationParameters.VerbosityOption);
	}
}
