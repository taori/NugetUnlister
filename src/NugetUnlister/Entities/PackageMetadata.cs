namespace NugetUnlister.Entities;

internal class PackageMetadata
{
	public PackageMetadata(string version, bool listed)
	{
		Version = version;
		Listed = listed;
	}

	public string Version { get; }

	public bool Listed { get; }

	public override string ToString()
	{
		return $"{Listed} {Version}";
	}
}
