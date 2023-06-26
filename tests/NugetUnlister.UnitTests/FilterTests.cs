using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NugetUnlister.Helpers;
using NugetUnlister.UnitTests.Utilities;
using Shouldly;
using VerifyXunit;
using Xunit;

namespace NugetUnlister.UnitTests;

[UsesVerify]
public class FilterTests
{
	[Theory]
	[InlineData("0.4.0", "TestContent.input.filterTests.test1.json",
		"TestContent.output.filterTests.prerelease.json")]
	[InlineData("0.4.0+147", "TestContent.input.filterTests.test1.json",
		"TestContent.output.filterTests.prerelease.json")]
	[InlineData("0.4.0-alpha1", "TestContent.input.filterTests.test1.json",
		"TestContent.output.filterTests.prerelease.json")]
	[InlineData("0.4.0-alpha1.147", "TestContent.input.filterTests.test1.json",
		"TestContent.output.filterTests.prerelease.json")]
	[InlineData("0.4.0-alpha1+147", "TestContent.input.filterTests.test1.json",
		"TestContent.output.filterTests.prerelease.json")]
	public async Task TestInputs_pre_release(string semVer, string inputFile, string outputFile)
	{
		var input = await FileUtility.GetEmbeddedJsonAsync<string[]>(inputFile);
		var converted = PackageHelper.FilterBefore(new HashSet<string>(input), semVer, true);
		var output = await FileUtility.GetEmbeddedJsonAsync<string[]>(outputFile);
		var expected = converted.Select(s => s.version.ToString()).ToArray();

		expected.ShouldBe(output);
	}

	[Theory]
	[InlineData("0.4.0", "TestContent.input.filterTests.test1.json", "TestContent.output.filterTests.release.json")]
	[InlineData("0.4.0+147", "TestContent.input.filterTests.test1.json",
		"TestContent.output.filterTests.release.json")]
	[InlineData("0.4.0-alpha1", "TestContent.input.filterTests.test1.json",
		"TestContent.output.filterTests.release.json")]
	[InlineData("0.4.0-alpha1.147", "TestContent.input.filterTests.test1.json",
		"TestContent.output.filterTests.release.json")]
	[InlineData("0.4.0-alpha1+147", "TestContent.input.filterTests.test1.json",
		"TestContent.output.filterTests.release.json")]
	public async Task TestInputs_release(string semVer, string inputFile, string outputFile)
	{
		var input = await FileUtility.GetEmbeddedJsonAsync<string[]>(inputFile);
		var converted = PackageHelper.FilterBefore(new HashSet<string>(input), semVer, false);
		var output = await FileUtility.GetEmbeddedJsonAsync<string[]>(outputFile);
		var expected = converted.Select(s => s.version.ToString()).ToArray();

		expected.ShouldBe(output);
	}


	[Theory]
	[InlineData("alpha00", null, "TestContent.input.filterTests.test1.json")]
	[InlineData("0.2.4", null, "TestContent.input.filterTests.test1.json")]
	[InlineData("0.2.4", true, "TestContent.input.filterTests.test1.json")]
	[InlineData("0.2.4", false, "TestContent.input.filterTests.test1.json")]
	public async Task PatternFilter(string pattern, bool? pre, string inputFile)
	{
		var input = await FileUtility.GetEmbeddedJsonAsync<string[]>(inputFile);
		var converted = PackageHelper.FilterPattern(new HashSet<string>(input), pattern, pre);
		var expected = converted.Select(s => s.version.ToString()).ToArray();

		await Verifier.Verify(expected)
			.UseParameters(pattern, pre, inputFile);
	}
}