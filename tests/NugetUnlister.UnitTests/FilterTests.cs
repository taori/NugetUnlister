using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NugetUnlister.UnitTests.Utilities;
using Shouldly;
using Xunit;

namespace NugetUnlister.UnitTests
{
	public class FilterTests
	{
		[Theory]
		[InlineData("0.4.0", "TestContent.input.filterTests.test1.json", "TestContent.output.filterTests.test1.json")]
		[InlineData("0.4.0.147", "TestContent.input.filterTests.test1.json", "TestContent.output.filterTests.test1.json")]
		[InlineData("0.4.0-alpha1", "TestContent.input.filterTests.test1.json", "TestContent.output.filterTests.test1.json")]
		public async Task TestInputs(string semVer, string inputFile, string outputFile)
		{
			var input = await FileUtility.GetEmbeddedJsonAsync<string[]>(inputFile);
			var converted = PackageHelper.FilterBefore(new HashSet<string>(input), semVer);
			var output = await FileUtility.GetEmbeddedJsonAsync<string[]>(outputFile);
			var expected = converted.Select(s => s.version.ToString()).ToArray();

			expected.ShouldBe(output);
		}
	}
}
