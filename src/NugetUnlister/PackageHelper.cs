﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Semver;

namespace NugetUnlister
{
	public static class PackageHelper
	{
		public static async Task<HashSet<string>> GetPackagesAsync(string packageName)
		{
//			var repository = PackageRepositoryFactory.Default.CreateRepository(repositoryUrl);
//			var packages = repository.FindPackagesById(packageName).Select(s => s.Version.ToFullString());
//			return new HashSet<string>(packages);

			string content;
			using (var client = new HttpClient())
			{
				var url = $@"https://www.nuget.org/packages/{packageName}/";
				Console.WriteLine($"Loading packages from \"{url}\".");
				content = await client.GetStringAsync(url).ConfigureAwait(false);
			}

			var regex = new Regex(@"/packages/"+ Regex.Escape(packageName) + @"/(?=\d)(?<id>[^""]+)(?<=\d)", RegexOptions.Compiled, TimeSpan.FromSeconds(10));
			var results = new HashSet<string>(regex.Matches(content).Select(s => s.Groups["id"].Value));
			return results;
		}

		public static IEnumerable<(string input, SemVersion version)> FilterBefore(HashSet<string> matches, string comparand, bool pre)
		{
			if (!GetSemanticVersion(comparand, out var sourceSemVer))
				yield break;

            var items = matches.Select(s => new
                {
                    input = s,
                    success = SemVersion.TryParse(s, out var sem),
                    version = sem
                })
                .Where(d => d.success).Select(s => (s.input, s.version))
				.OrderByDescending(d => d.version, Comparer<SemVersion>.Default)
				.Where(d => d.version.CompareTo(sourceSemVer) <= 0 && (pre && !string.IsNullOrEmpty(d.version.Prerelease) || !pre && string.IsNullOrEmpty(d.version.Prerelease)))
                .Select(d => (d.input, d.version));

            foreach (var tuple in items)
			{
				yield return tuple;
			}
		}

		private static readonly Regex SemverRegex = new Regex("^[\\d]+\\.[\\d]+\\.[\\d]+", RegexOptions.Compiled);

		private static bool GetSemanticVersion(string comparand, out SemVersion sourceSemVer)
		{
			if (SemVersion.TryParse(comparand, out sourceSemVer))
			{
				return true;
			}
			else
			{
				Console.WriteLine($@"""{comparand}"" can not be parsed as semantic version.");
				var match = SemverRegex.Match(comparand);
				if (match.Success)
				{
					if (SemVersion.TryParse(match.Value, out sourceSemVer))
					{
						Console.WriteLine($@"""{comparand}"" was parsed as {sourceSemVer}.");
						return true;
					}
					else
					{
						Console.WriteLine($@"""{comparand}"" can not be parsed as regex.");
						return false;
					}
				}
				else
				{
					Console.WriteLine($@"""{comparand}"" can not be parsed as semantic version.");
					return false;
				}
			}
		}
	}
}