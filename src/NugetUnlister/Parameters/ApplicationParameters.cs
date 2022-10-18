namespace NugetUnlister.Parameters;

public static class ApplicationParameters
{
	public static readonly PackageNameArgument PackageNameArgument = new();
	public static readonly VersionArgument VersionArgument = new();
	public static readonly ApiKeyArgument ApiKeyArgument = new();
	public static readonly SourceServerOption SourceServerOption = new();
	public static readonly RegexArgument RegexArgument = new();
}
