using System.CommandLine;
using Microsoft.Extensions.Logging;
using NugetUnlister.Entities;

namespace NugetUnlister.Parameters;

public class VerbosityOption : Option<LogLevel>
{
	public VerbosityOption() : base(["-v", "--verbosity"], () => LogLevel.Information, "Verbosity level of the application")
	{
	}
}
