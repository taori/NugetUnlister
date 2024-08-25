namespace NugetUnlister;

public class CliUnavailableException : ExitCodeException
{
	public CliUnavailableException() : base(-100, $"Cli is not available")
	{
	}
}
