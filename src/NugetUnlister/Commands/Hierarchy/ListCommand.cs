using System.CommandLine;

namespace NugetUnlister.Commands.Hierarchy;

public class ListCommand : Command
{
	public ListCommand() : base("list", "entry point for list commands")
	{
		AddCommand(new ListAllCommand());
		AddCommand(new ListReleaseBefore());
		AddCommand(new ListPrereleaseBefore());
	}
}