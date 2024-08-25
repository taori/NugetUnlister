using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using NugetUnlister.Interfaces;
using NugetUnlister.Parameters;
using NugetUnlister.Scopes;

namespace NugetUnlister.Commands;

public class ListLike : Command
{
	public ListLike() : base("like", "unlists release versions prior to given version")
	{
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddArgument(ApplicationParameters.RegexArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (packageName, pattern, packageSource, verbosity) =>
		{
			var api = ServiceScope.Current?.ServiceProvider.GetRequiredService<IApiCalls>() ?? throw new CliUnavailableException();
			await api.ListAsync(verbosity, packageName, null, pattern, false, packageSource);
		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.RegexArgument, ApplicationParameters.SourceServerOption, ApplicationParameters.VerbosityOption);
	}
}
