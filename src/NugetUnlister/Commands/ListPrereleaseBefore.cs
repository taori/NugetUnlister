using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using NugetUnlister.Interfaces;
using NugetUnlister.Parameters;
using NugetUnlister.Scopes;

namespace NugetUnlister.Commands;

public class ListPrereleaseBefore : Command
{
	public ListPrereleaseBefore() : base("PrereleaseBefore", "unlists prerelease versions prior to given version")
	{
		AddAlias("pre");
		AddAlias("prereleasebefore");
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddArgument(ApplicationParameters.VersionArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (packageName, version, packageSource, verbosity) =>
		{
			var api = ServiceScope.Current?.ServiceProvider.GetRequiredService<IApiCalls>() ?? throw new CliUnavailableException();
			await api.ListAsync(verbosity, packageName, version, null, true, packageSource);
		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.VersionArgument, ApplicationParameters.SourceServerOption, ApplicationParameters.VerbosityOption);
	}
}
