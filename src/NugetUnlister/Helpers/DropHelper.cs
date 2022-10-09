using CliWrap;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using CliWrap.Buffered;

namespace NugetUnlister.Helpers;

internal class DropHelper
{
	internal static async Task<int> DropBefore(string package, string version, string apiKey, string src, bool pre)
	{
		if (package == null)
			throw new ExitCodeException(1, $"{nameof(package)} is missing");
		if (version == null)
			throw new ExitCodeException(2, $"{nameof(version)} is missing");
		if (apiKey == null)
			throw new ExitCodeException(3, $"{nameof(apiKey)} is missing");

		try
		{
			var matches = await PackageHelper.GetPackagesAsync(package);
			var filtered = PackageHelper.FilterBefore(matches, version, pre).ToArray();
			Console.WriteLine($"Found {filtered.Length} packages below version {version}.");
			foreach (var match in filtered)
			{
				try
				{
					string arguments;
					string logArguments;
					if (src == null)
					{
						arguments =
							$"nuget delete \"{package}\" \"{match.input}\" --non-interactive --force-english-output -k {apiKey}";
						logArguments =
							$"nuget delete \"{package}\" \"{match.input}\" --non-interactive --force-english-output -k ***";
					}
					else
					{
						arguments =
							$"nuget delete \"{package}\" \"{match.input}\" --non-interactive --force-english-output -k {apiKey} -s {src}";
						logArguments =
							$"nuget delete \"{package}\" \"{match.input}\" --non-interactive --force-english-output -k *** -s {src}";
					}


					Console.WriteLine($"Executing {logArguments}");

					using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
					var command = await Cli.Wrap("dotnet")
						.WithArguments(arguments)
						.WithValidation(CommandResultValidation.None)
						.ExecuteBufferedAsync(cts.Token);

					var c = Console.ForegroundColor;
					Console.ForegroundColor = command.StandardOutput.Contains("successful") ? ConsoleColor.Green : ConsoleColor.Red;
					Console.Write(command.StandardOutput);
					Console.ForegroundColor = c;
					if (command.ExitCode != 0)
					{
						throw new ExitCodeException(6, "Nuget process failed");
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw new ExitCodeException(5, "Process failed", e);
				}
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw new ExitCodeException(4, "Unknown error occured", e);
		}

		return 0;
	}

}
