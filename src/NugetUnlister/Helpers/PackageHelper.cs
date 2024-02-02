using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semver;

namespace NugetUnlister.Helpers;

public static class PackageHelper
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="packageName"></param>
	/// <param name="feedUrl">Url similar to as https://api.nuget.org/v3-flatcontainer/</param>
	/// <returns></returns>
	/// <exception cref="ExitCodeException"></exception>
	public static async Task<HashSet<string>> GetPackagesAsync(string packageName, string? feedUrl)
	{
		string content;
		string url;
		using (var client = new HttpClient())
		{
			url = feedUrl == null
				? $@"https://api.nuget.org/v3-flatcontainer/{packageName}/index.json"
				: $@"{feedUrl.TrimEnd('/')}/{packageName}/index.json";

			Console.WriteLine($"Loading packages from \"{url}\".");
			content = await client.GetStringAsync(url).ConfigureAwait(false);
		}

		var response = JsonConvert.DeserializeObject<ResponseOfV3Flat>(content);
		if (response is null)
			throw new ExitCodeException(404, $"Failed to get content for {url}");

		var results = new HashSet<string>(response.versions);
		return results;
	}

	public static IEnumerable<(string input, SemVersion version)> FilterBefore(HashSet<string> matches, string comparand, bool? pre)
	{
		if (!SemVersion.TryParse(comparand, out var sourceSemVer))
			yield break;

		var items = matches.Select(s => new {input = s, success = SemVersion.TryParse(s, out var sem), version = sem})
			.Where(d => d.success)
			.Select(s => (s.input, s.version))
			.OrderByDescending(d => d.version, Comparer<SemVersion>.Default)
			.Where(d => d.version.CompareTo(sourceSemVer) <= 0 && (!pre.HasValue || (string.IsNullOrEmpty(d.version.Prerelease) != pre.Value)));

		foreach (var tuple in items)
		{
			yield return tuple;
		}
	}

	public static IEnumerable<(string input, SemVersion version)> FilterPattern(HashSet<string> matches, string pattern, bool? pre)
	{
		var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));
		// recycle logic - return everything
		// SemVersion did not have a public max number but 100000.0.0 should be save to use for releases of chome until 2025
		return FilterBefore(matches, "100000.0.0", pre)
			.Where(d => regex.IsMatch(d.input));
	}
}


internal class ResponseOfV3Flat
{
	public string[] versions { get; set; }
}
