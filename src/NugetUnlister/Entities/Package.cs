using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NugetUnlister.Requests;
using Semver;

namespace NugetUnlister.Entities;

internal class Package
{
	public PackageMetadata[] Metadata { get; set; } = [];

	private void ApplyFilter(string? pattern, bool? prerelease, string? version)
	{
		var regex = pattern != null
			? new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1))
			: null;

		var targetVersion = version is null
			? SemVersion.Parse("100000.0.0", SemVersionStyles.Any) // this should be good until 2026 to parse chrome versions
			: SemVersion.TryParse(version, SemVersionStyles.Any, out var v)
				? v
				: throw new InvalidVersionException(version);
		
		var items = Metadata.Select(metadata => new
				{
					input = metadata,
					success = SemVersion.TryParse(metadata.Version, SemVersionStyles.Any, out var sem),
					version = sem
				}
			)
			.Where(d => d.success)
			.Select(s => (s.input, s.version))
			.OrderByDescending(d => d.version, Comparer<SemVersion>.Default)
			.Where(d => regex is null || regex.IsMatch(d.input.Version))
			.Where(d => prerelease is null || d.version.IsPrerelease == prerelease.Value)
			.Where(d => d.version.ComparePrecedenceTo(targetVersion) < 0);

		Metadata = items.Select(d => d.input).ToArray();
	}

	public void Filter(ListRequest request)
	{
		ApplyFilter(request.Pattern, request.Prerelease, request.Version);
	}

	public void Filter(DropBeforeRequest request)
	{
		ApplyFilter(null, request.Prerelease, request.Version);
	}

	public void Filter(DropLikeRequest request)
	{
		ApplyFilter(request.Pattern, request.Prerelease, null);
	}
}
