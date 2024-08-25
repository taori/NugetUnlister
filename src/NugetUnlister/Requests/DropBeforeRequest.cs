namespace NugetUnlister.Requests;

internal record DropBeforeRequest(
	string PackageName,
	string Version,
	string ApiKey,
	string PackageSource,
	bool? Prerelease
);