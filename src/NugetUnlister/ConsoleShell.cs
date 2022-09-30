using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using CommandDotNet.Attributes;
using NugetUnlister.Utility;

namespace NugetUnlister
{
	[ApplicationMetadata(
		Name = "nuget-unlist")]
	public class ConsoleShell
	{
		[SubCommand]
		[ApplicationMetadata(
			Description = "List commands")]
		public class List
		{
			[ApplicationMetadata(
				Description = "Lists all packages for a package.",
				Syntax = "list all [packageName]")]
			public async Task<int> All(
				string package)
			{
				try
				{
					var matches = await PackageHelper.GetPackagesAsync(package);
					foreach (var match in matches)
					{
						Console.WriteLine(match);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					return 1;
				}

				return 0;
			}

			[ApplicationMetadata(
				Description = "Lists all prereleases before given version",
				Syntax = "list prereleasebefore [package] [1.0.3]")]
			public async Task<int> PrereleaseBefore(
				[Argument(Name = "package", Description = "Package name")]
				string package,
				[Argument(Name = "version", Description = "Semantic version")]
				string version)
			{
				return await ListBefore(package, version, true);
			}

			[ApplicationMetadata(
				Description = "Lists all releases before given version",
				Syntax = "list releasebefore [package] [1.0.3]")]
			public async Task<int> ReleaseBefore(
				[Argument(Name = "package", Description = "Package name")]
				string package,
				[Argument(Name = "version", Description = "Semantic version")]
				string version)
			{
				return await ListBefore(package, version, false);
			}

			private static async Task<int> ListBefore(string package, string version, bool pre)
			{
				if (package == null)
					throw new ExitCodeException(1, $"{nameof(package)} is missing");
				if (version == null)
					throw new ExitCodeException(2, $"{nameof(version)} is missing");

				try
				{
					var matches = await PackageHelper.GetPackagesAsync(package);
					var filtered = PackageHelper.FilterBefore(matches, version, pre);
					foreach (var match in filtered)
					{
						Console.WriteLine(match.input);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					return 3;
				}

				return 0;
			}
		}

		[SubCommand]
		[ApplicationMetadata(
			Description = "Methods used for unlisting packages")]
		public class Drop
		{
			[ApplicationMetadata(
				Description = "Unlists first all prerelease and then all release versions before given semantic version.",
				Syntax = "drop anyreleasebefore [package] [version] [apiKey] [packageSource]")]
			public async Task<int> AnyReleaseBefore(
				[Argument(Name = "package name", Description = "package name")]
				string package,
				[Argument(Name = "semantic version", Description = "Semantic version to compare against")]
				string version,
				[Argument(Name = "api key", Description = "ApiKey for package")]
				string apiKey,
				[Argument(Name = "package source",
					Description =
						"Repository source, e.g. https://www.nuget.org, https://www.nuget.org/api/v3, https://www.nuget.org/api/v2/package (see https://docs.microsoft.com/de-de/dotnet/core/tools/dotnet-nuget-delete?tabs=netcore2x)")]
				string src = "https://www.nuget.org")
			{
				var exitCode = await DropBefore(package, version, apiKey, src, pre: true);

				if (exitCode != 0) {
					// Premature exit due to non-zero exit code.
					return exitCode;
				}

				return await DropBefore(package, version, apiKey, src, pre: false);
			}

			[ApplicationMetadata(
				Description = "Unlists all prerelease versions before given semantic version.",
				Syntax = "drop prereleasebefore [package] [version] [apiKey] [packageSource]")]
			public async Task<int> PrereleaseBefore(
				[Argument(Name = "package name", Description = "package name")]
				string package,
				[Argument(Name = "semantic version", Description = "Semantic version to compare against")]
				string version,
				[Argument(Name = "api key", Description = "ApiKey for package")]
				string apiKey,
				[Argument(Name = "package source",
					Description =
						"Repository source, e.g. https://www.nuget.org, https://www.nuget.org/api/v3, https://www.nuget.org/api/v2/package (see https://docs.microsoft.com/de-de/dotnet/core/tools/dotnet-nuget-delete?tabs=netcore2x)")]
				string src = "https://www.nuget.org")
			{
				return await DropBefore(package, version, apiKey, src, pre: true);
			}

			[ApplicationMetadata(
				Description = "Unlists all prerelease versions before given semantic version.",
				Syntax = "drop releasebefore [package] [version] [apiKey] [packageSource]")]
			public async Task<int> ReleaseBefore(
				[Argument(Name = "package name", Description = "package name")]
				string package,
				[Argument(Name = "semantic version", Description = "Semantic version to compare against")]
				string version,
				[Argument(Name = "api key", Description = "ApiKey for package")]
				string apiKey,
				[Argument(Name = "package source",
					Description =
						"Repository source, e.g. https://www.nuget.org, https://www.nuget.org/api/v3, https://www.nuget.org/api/v2/package (see https://docs.microsoft.com/de-de/dotnet/core/tools/dotnet-nuget-delete?tabs=netcore2x)")]
				string src = "https://www.nuget.org")
			{
				return await DropBefore(package, version, apiKey, src, pre: false);
			}

			private static async Task<int> DropBefore(string package, string version, string apiKey, string src,
				bool pre)
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
							using (var process = new Process())
							{
								process.StartInfo.UseShellExecute = true;
								process.StartInfo.FileName = "dotnet";
								string arguments;
								string logArguments;
								if (src == null)
								{
									arguments =
										$"nuget delete \"{package}\" \"{match.input}\" --non-interactive -k {apiKey}";
									logArguments =
										$"nuget delete \"{package}\" \"{match.input}\" --non-interactive -k ***";
								}
								else
								{
									arguments =
										$"nuget delete \"{package}\" \"{match.input}\" --non-interactive -k {apiKey} -s {src}";
									logArguments =
										$"nuget delete \"{package}\" \"{match.input}\" --non-interactive -k *** -s {src}";
								}


								Console.WriteLine($"Executing {logArguments}");

								using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
								var command = await Cli.Wrap("dotnet")
									.WithArguments(arguments)
									.ExecuteBufferedAsync(cts.Token);

								if (command.ExitCode != 0)
								{
									Console.WriteLine("Error:");
									Console.WriteLine(command.StandardError);
									Console.WriteLine("Output:");
									Console.WriteLine(command.StandardOutput);
									throw new ExitCodeException(6, "Nuget process failed");
								}
								else
								{
									Console.WriteLine(command.StandardOutput);
								}
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
	}
}
