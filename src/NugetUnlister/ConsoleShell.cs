using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommandDotNet.Attributes;

namespace NugetUnlister
{
	public class ConsoleShell
	{
		[SubCommand]
		public class List
		{
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

			public async Task<int> PrereleaseBefore(
				[Option(ShortName = "p", Description = "Package name")]
				string package,
				[Option(LongName = "sv", Description = "Semantic version")]
				string version)
			{
				if (package == null)
					throw new ExitCodeException(1, nameof(package));
				if (version == null)
					throw new ExitCodeException(2, nameof(version));

				try
				{
					var matches = await PackageHelper.GetPackagesAsync(package);
					var filtered = PackageHelper.FilterBefore(matches, version);
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
		public class Drop
		{
			public async Task<int> PrereleaseBefore(
				[Option(ShortName = "p", Description = "package name")]
				string package,
				[Option(LongName = "sv", Description = "Semantic version to compare against")]
				string version,
				[Option(ShortName = "k", Description = "ApiKey for package")]
				string apiKey,
				[Option(ShortName = "s", Description = "Repository source, e.g. https://www.nuget.org, https://www.nuget.org/api/v3, https://www.nuget.org/api/v2/package (see https://docs.microsoft.com/de-de/dotnet/core/tools/dotnet-nuget-delete?tabs=netcore2x)")]
				string src = "https://www.nuget.org")
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
					var filtered = PackageHelper.FilterBefore(matches, version).ToArray();
					Console.WriteLine($"Found {filtered.Length} packages below version {version}.");
					foreach (var match in filtered)
					{
						try
						{
							using (var process = new Process())
							{
								process.StartInfo.FileName = "nuget.exe";
								string arguments;
								if (src == null)
								{
									arguments = $"delete {package} {match.input} -NonInteractive -Verbosity n -ApiKey {apiKey}";
								}
								else
								{
									arguments = $"delete {package} {match.input} -NonInteractive -Verbosity n -ApiKey {apiKey} -Source {src}";
								}
								process.StartInfo.Arguments = arguments;
								Console.WriteLine($"Executing nuget {arguments}");
								process.Start();
								process.WaitForExit();
								var error = await process.StandardError.ReadToEndAsync();
								if (process.ExitCode != 0)
								{
									Console.ForegroundColor = ConsoleColor.Red;
									Console.WriteLine(error);
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