using System.Text.Json;
using System.Threading.Tasks;
using NugetUnlister.Entities;
using NugetUnlister.Requests;
using NugetUnlister.UnitTests.Utilities;
using VerifyXunit;
using Xunit;

namespace NugetUnlister.UnitTests.Tests;

[UsesVerify]
public class PackageFilterTests
{
	[Fact(Timeout = 1000)]
	public async Task ListAll()
	{
		var package = GetDefaultPackage();
		await Verifier.Verify(package);
	}
	
	[Theory(Timeout = 1000)]
	[InlineData("0.5.0")]
	[InlineData("0.1.0-alpha.3")]
	[InlineData("0.1.0-alpha.4")]
	[InlineData("0.1.0-beta.3")]
	[InlineData("0.1.0")]
	[InlineData("1.0.0")]
	[InlineData("1.0.0-beta")]
	[InlineData("1.0.3-alpha")]
	public async Task ListAllJustVersion(string version)
	{
		var package = GetDefaultPackage();
		package.Filter(new ListRequest(string.Empty, version, null, null, string.Empty));
		
		await Verifier
			.Verify(package)
			.UseParameters(version);
	}
	
	[Theory(Timeout = 1000)]
	[InlineData("0.5.0")]
	[InlineData("0.1.0-alpha.3")]
	[InlineData("0.1.0-alpha.4")]
	[InlineData("0.1.0-beta.3")]
	[InlineData("0.1.0")]
	[InlineData("1.0.0")]
	[InlineData("1.0.0-beta")]
	[InlineData("1.0.3-alpha")]
	public async Task ListAllVersionPreRelease(string version)
	{
		var package = GetDefaultPackage();
		package.Filter(new ListRequest(string.Empty, version, null, true, string.Empty));
		
		await Verifier
			.Verify(package)
			.UseParameters(version);
	}
	
	[Theory(Timeout = 1000)]
	[InlineData("0.5.0")]
	[InlineData("0.1.0-alpha.3")]
	[InlineData("0.1.0-alpha.4")]
	[InlineData("0.1.0-beta.3")]
	[InlineData("0.1.0")]
	[InlineData("1.0.0")]
	[InlineData("1.0.0-beta")]
	[InlineData("1.0.3-alpha")]
	[InlineData("1.0.2")]
	public async Task ListAllVersionFullRelease(string version)
	{
		var package = GetDefaultPackage();
		package.Filter(new ListRequest(string.Empty, version, null, false, string.Empty));
		
		await Verifier
			.Verify(package)
			.UseParameters(version);
	}
	
	[Theory(Timeout = 1000)]
	[InlineData("^0.1.0")]
	[InlineData("alpha")]
	[InlineData("beta")]
	[InlineData("\\.3$")]
	[InlineData("\\.3")]
	public async Task ListPattern(string pattern)
	{
		var package = GetDefaultPackage();
		package.Filter(new ListRequest(string.Empty, null, pattern, null, string.Empty));
		
		await Verifier
			.Verify(package)
			.UseParameters(pattern);
	}

	private static Package GetDefaultPackage()
	{
		var embeddedResourceReader = new EmbeddedResourceReader(typeof(PackageProviderTests).Assembly);
		var json = embeddedResourceReader.GetContent("TestContent.input.packagesample.json");
		return JsonSerializer.Deserialize<Package>(json);
	}
}