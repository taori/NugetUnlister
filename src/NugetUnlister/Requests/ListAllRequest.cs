namespace NugetUnlister.Requests;

internal record ListAllRequest(
	string PackageName,
	string PackageSource
);