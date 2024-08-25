namespace NugetUnlister.Requests;

internal record DropLikeRequest(
	string PackageName,
	string Pattern,
	string ApiKey,
	string PackageSource,
	bool? Prerelease
);