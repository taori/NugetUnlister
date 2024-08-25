using System;
using System.CommandLine;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NugetUnlister.Interfaces;
using NugetUnlister.Parameters;
using NugetUnlister.Scopes;

namespace NugetUnlister.Commands;

public class ListAllCommand : Command
{
	public ListAllCommand() : base("all", "lists all versions for a package - useful to verify targets for other commands")
	{
		AddAlias("any");
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (package, packageSource, verbosity) =>
		{
			var api = ServiceScope.Current?.ServiceProvider.GetRequiredService<IApiCalls>() ?? throw new CliUnavailableException();
			await api.ListAllAsync(verbosity, package, packageSource);
		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.SourceServerOption, ApplicationParameters.VerbosityOption);
	}
}
