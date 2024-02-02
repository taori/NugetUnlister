using System;
using System.Linq;
using System.Threading.Tasks;

namespace NugetUnlister.Helpers;

internal static class ListHelper
{
	internal static async Task<int> ListAsync(string packageName, string version, bool pre, string packageSource)
	{
		if (packageName == null)
			throw new ExitCodeException(1, $"{nameof(packageName)} is missing");
		if (version == null)
			throw new ExitCodeException(2, $"{nameof(version)} is missing");

		try
		{
			var matches = await PackageHelper.GetPackagesAsync(packageName, packageSource);
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

	public static async Task<int> ListPatternAsync(string packageName, string pattern, bool? pre, string packageSource)
	{
		try
		{
			var matches = await PackageHelper.GetPackagesAsync(packageName, packageSource);
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
