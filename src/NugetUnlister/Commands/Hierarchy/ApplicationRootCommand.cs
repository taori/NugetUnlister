using System.CommandLine;

namespace NugetUnlister.Commands.Hierarchy;

public class ApplicationRootCommand : RootCommand
{
	public ApplicationRootCommand()
	{
		AddCommand(new ListCommand());
		AddCommand(new DropCommand());
	}
}
