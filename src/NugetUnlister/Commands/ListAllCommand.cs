using System;
using System.CommandLine;
using System.Threading.Tasks;
using NugetUnlister.Helpers;
using NugetUnlister.Parameters;

namespace NugetUnlister.Commands;

public class ListAllCommand : Command
{
	public ListAllCommand() : base("all", "lists all versions for a package - useful to verify targets for other commands")
	{
		AddAlias("any");
		AddArgument(ApplicationParameters.PackageNameArgument);
		AddOption(ApplicationParameters.SourceServerOption);

		this.SetHandler(async (package, packageSource) =>
		{
			await ExecuteAsync(package, packageSource);
		}, ApplicationParameters.PackageNameArgument, ApplicationParameters.SourceServerOption);
	}

	internal static async Task ExecuteAsync(string packageName, string packageSource)
	{
		try
		{
			var matches = await PackageHelper.GetPackagesAsync(packageName, packageSource);
			foreach (var match in matches)
			{
				Console.WriteLine(match);
			}
		}
		catch (Exception e)
		{
			throw new ExitCodeException(1, e.Message, e);
		}
	}
}
