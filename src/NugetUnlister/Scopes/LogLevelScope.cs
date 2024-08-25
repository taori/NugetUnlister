using Amusoft.Toolkit.Threading;
using Microsoft.Extensions.Logging;

namespace NugetUnlister.Scopes;

public class LogLevelScope : AmbientScope<LogLevelScope>
{
	public LogLevel Level { get; }

	public LogLevelScope(LogLevel level)
	{
		Level = level;
	}
}
