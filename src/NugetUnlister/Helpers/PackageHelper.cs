using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semver;

namespace NugetUnlister.Helpers
{
	public static class PackageHelper
	{
		public static async Task<HashSet<string>> GetPackagesAsync(string packageName)
		{
			string content;
			string url;
			using (var client = new HttpClient())
			{
				url = $@"https://api.nuget.org/v3-flatcontainer/{packageName}/index.json";
				Console.WriteLine($"Loading packages from \"{url}\".");
				content = await client.GetStringAsync(url).ConfigureAwait(false);
			}

			var response = JsonConvert.DeserializeObject<ResponseOfV3Flat>(content);
			if (response is null)
				throw new ExitCodeException(404, $"Failed to get content for {url}");

			var results = new HashSet<string>(response.versions);
			return results;
		}

		public static IEnumerable<(string input, SemVersion version)> FilterBefore(HashSet<string> matches, string comparand, bool pre)
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


	internal class ResponseOfV3Flat
	{
		public string[] versions { get; set; }
	}
}
