using System;
using System.Threading;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using Microsoft.Extensions.Logging;
using NugetUnlister.Interfaces;

namespace NugetUnlister.Services;

public class NugetCli(
	IEnvironmentInformation environmentInformation,
	ILogger logger
) : INugetCli
{
	public async Task DropAsync(string package, string version, string apiKey)
	{
		var arguments = $"nuget delete \"{package}\" \"{version}\" --non-interactive --force-english-output -k {apiKey} -s {environmentInformation.SymbolSource}";
		using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
		logger.LogDebug($"Executing dotnet {arguments}");
		
		var command = await Cli.Wrap("dotnet")
			.WithArguments(arguments)
			.WithValidation(CommandResultValidation.None)
			.ExecuteBufferedAsync(cts.Token);
		
		if (command.StandardOutput.Contains("successful"))
		{
			logger.LogInformation(command.StandardOutput);
		}
		else
		{
			logger.LogError(command.StandardOutput);
		}
	}
}
