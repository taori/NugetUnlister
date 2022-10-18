using System;
using System.Linq;
using System.Threading.Tasks;

namespace NugetUnlister.Helpers;

internal static class ListHelper
{
	internal static async Task<int> ListAsync(string package, string version, bool pre)
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
			throw new ExitCodeException(3, e.Message, e);
		}

		return 0;
	}

	public static async Task<int> ListPatternAsync(string package, string pattern, bool? pre)
	{
		try
		{
			var matches = await PackageHelper.GetPackagesAsync(package);
			var filtered = PackageHelper.FilterPattern(matches, pattern, pre).ToArray();
			foreach (var match in filtered)
			{
				Console.WriteLine(match.input);
			}
		}
		catch (Exception e)
		{
			throw new ExitCodeException(3, e.Message, e);
		}

		return 0;
	}
}
