using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using NugetUnlister.Interfaces;
using NugetUnlister.Parameters;
using NugetUnlister.Scopes;

namespace NugetUnlister.Commands;

public class ListReleaseBefore : Command
{
	public ListReleaseBefore() : base("ReleaseBefore", "unlists release versions prior to given version")
	{
		AddAlias("rel");
		AddAlias("releasebefore");
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddArgument(ApplicationParameters.VersionArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (packageName, version, packageSource, verbosity) =>
		{
			var api = ServiceScope.Current?.ServiceProvider.GetRequiredService<IApiCalls>() ?? throw new CliUnavailableException();
			await api.ListAsync(verbosity, packageName, version, null, false, packageSource);
		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.VersionArgument, ApplicationParameters.SourceServerOption, ApplicationParameters.VerbosityOption);
	}
}
