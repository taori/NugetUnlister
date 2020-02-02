using System;
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
			string content;
			using (var client = new HttpClient())
			{
				var url = $@"https://www.nuget.org/packages/{packageName}/";
				Console.WriteLine($"Loading packages from \"{url}\".");
				content = await client.GetStringAsync(url).ConfigureAwait(false);
			}

			var regex = new Regex(@"/packages/" + Regex.Escape(packageName) + @"/(?=\d)(?<id>[^""]+)(?<=\d)",
				RegexOptions.Compiled, TimeSpan.FromSeconds(10));
			var results = new HashSet<string>(regex.Matches(content).Select(s => s.Groups["id"].Value));
			return results;
		}

		public static IEnumerable<(string input, SemVersion version)> FilterBefore(HashSet<string> matches,
			string comparand, bool pre)
		{
			if (!SemVersion.TryParse(comparand, out var sourceSemVer))
				yield break;

			var items = matches.Select(s => new
				{
					input = s,
					success = SemVersion.TryParse(s, out var sem),
					version = sem
				})
				.Where(d => d.success)
				.Select(s => (s.input, s.version))
				.OrderByDescending(d => d.version, Comparer<SemVersion>.Default)
				.Where(d => d.version.CompareTo(sourceSemVer) <= 0 &&
				            (pre && !string.IsNullOrEmpty(d.version.Prerelease) ||
				             !pre && string.IsNullOrEmpty(d.version.Prerelease)));

			foreach (var tuple in items)
			{
				yield return tuple;
			}
		}
	}
}