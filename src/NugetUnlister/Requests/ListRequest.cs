namespace NugetUnlister.Requests;

internal record ListRequest(
	string PackageName,
	string? Version,
	string? Pattern,
	bool? Prerelease,
	string PackageSource
);
