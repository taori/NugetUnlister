using System.CommandLine;
using NugetUnlister.Parameters;

namespace NugetUnlister.Commands.Hierarchy;

public class ApplicationRootCommand : RootCommand
{
	public ApplicationRootCommand()
	{
		AddCommand(new ListCommand());
		AddCommand(new DropCommand());
		AddGlobalOption(ApplicationParameters.VerbosityOption);
	}
}
