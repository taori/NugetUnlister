using CliWrap;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using CliWrap.Buffered;
using Semver;

namespace NugetUnlister.Helpers;

internal class DropHelper
{
	internal static async Task<int> DropBefore(string package, string version, string apiKey, string src, bool pre)
	{
		return await DropInternal(package, version, apiKey, src, input => PackageHelper.FilterBefore(input, version, pre));
	}

	internal static async Task<int> DropLike(string package, string pattern, string apiKey, string src, bool? pre)
	{
		return await DropInternal(package, pattern, apiKey, src, input => PackageHelper.FilterPattern(input, pattern, pre));
	}

	private static async Task<int> DropInternal(string package, string queryIdentifier, string apiKey, string src, Func<HashSet<string>, IEnumerable<(string input, SemVersion version)>> filter)
	{
		if (package == null)
			throw new ExitCodeException(1, $"{nameof(package)} is missing");
		if (queryIdentifier == null)
			throw new ExitCodeException(2, $"{nameof(queryIdentifier)} is missing");
		if (apiKey == null)
			throw new ExitCodeException(3, $"{nameof(apiKey)} is missing");

		try
		{
			var matches = await PackageHelper.GetPackagesAsync(package, src);
			var filtered = filter(matches).ToArray();
			// var filtered = PackageHelper.FilterBefore(matches, version, pre).ToArray();
			Console.WriteLine($"Found {filtered.Length} for {queryIdentifier}.");
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
