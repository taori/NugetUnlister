using System;
using System.CommandLine;
using NugetUnlister.Helpers;
using NugetUnlister.Parameters;

namespace NugetUnlister.Commands;

public class ListAllCommand : Command
{
	public ListAllCommand() : base("all", "lists all versions for a package - useful to verify targets for other commands")
	{
		AddAlias("any");
		AddArgument(ApplicationParameters.PackageNameArgument);

		this.SetHandler(async (package) =>
		{
			try
			{
				var matches = await PackageHelper.GetPackagesAsync(package);
				foreach (var match in matches)
				{
					Console.WriteLine(match);
				}
			}
			catch (Exception e)
			{
				throw new ExitCodeException(1, e.Message, e);
			}
		}, ApplicationParameters.PackageNameArgument);
	}
}
