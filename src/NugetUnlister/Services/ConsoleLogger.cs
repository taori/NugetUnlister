using System;
using Microsoft.Extensions.Logging;
using NugetUnlister.Scopes;

namespace NugetUnlister.Services;

public class ConsoleLogger : ILogger
{
	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
	{
		var level = LogLevelScope.Current?.Level ?? LogLevel.Trace;
		if (level > logLevel)
			return;

		var color = logLevel switch
		{
			LogLevel.Trace => ConsoleColor.Gray,
			LogLevel.Debug => ConsoleColor.DarkGray,
			LogLevel.Information => ConsoleColor.White,
			LogLevel.Warning => ConsoleColor.Yellow,
			LogLevel.Error => ConsoleColor.Red,
			LogLevel.Critical => ConsoleColor.DarkRed,
			LogLevel.None => ConsoleColor.White,
			_ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
		};

		using (new ConsoleColorScope(color))
		{
			Console.WriteLine(SecretsScope.ScrubMessage(formatter(state, exception)));
		}
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return logLevel == (LogLevelScope.Current?.Level ?? LogLevel.Trace);
	}

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull
	{
		return null;
	}
}
