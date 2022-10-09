using System.CommandLine;

namespace NugetUnlister.Commands.Hierarchy;

public class DropCommand : Command
{
	public DropCommand() : base("drop", "entry point for drop commands")
	{
		AddCommand(new DropPrereleaseBefore());
		AddCommand(new DropReleaseBefore());
		AddCommand(new DropAnyReleaseBefore());
	}
}