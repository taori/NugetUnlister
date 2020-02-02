using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CommandDotNet.Attributes;

namespace NugetUnlister
{
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
					throw new ExitCodeException(1, nameof(package));
				if (version == null)
					throw new ExitCodeException(2, nameof(version));

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
				return await DropBefore(package, version, apiKey, src, true);
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
				return await DropBefore(package, version, apiKey, src, false);
			}

			private static async Task<int> DropBefore(string package, string version, string apiKey, string src,
				bool pre)
			{
				if (package == null)
					throw new ExitCodeException(1, nameof(package));
				if (version == null)
					throw new ExitCodeException(2, nameof(version));
				if (apiKey == null)
					throw new ExitCodeException(3, nameof(apiKey));

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
								process.StartInfo.FileName = "nuget.exe";
								string arguments;
								string logArguments;
								if (src == null)
								{
									arguments =
										$"delete {package} {match.input} -NonInteractive -Verbosity n -ApiKey {apiKey}";
									logArguments =
										$"delete {package} {match.input} -NonInteractive -Verbosity n -ApiKey ***";
								}
								else
								{
									arguments =
										$"delete {package} {match.input} -NonInteractive -Verbosity n -ApiKey {apiKey} -Source {src}";
									logArguments =
										$"delete {package} {match.input} -NonInteractive -Verbosity n -ApiKey *** -Source {src}";
								}

								process.StartInfo.Arguments = arguments;
								Console.WriteLine($"Executing nuget {logArguments}");
								process.Start();
								process.WaitForExit();
								if (process.ExitCode != 0)
								{
									Console.WriteLine($"nuget process exited with exitCode: {process.ExitCode}.");
									return process.ExitCode;
								}
							}
						}
						catch (Exception e)
						{
							Console.WriteLine(e);
							return 5;
						}
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					return 4;
				}

				return 0;
			}
		}
	}
}
