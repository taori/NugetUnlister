using NugetUnlister.Commands;
using NugetUnlister.IntegrationTests.Toolkit;

namespace NugetUnlister.IntegrationTests;

[UsesVerify]
public class CommandTests
{
	[Theory]
	[InlineData("SymbolSource.TestPackage", false)]
	[InlineData("symbolsource.testpackage", true)]
	public async Task ListAll(string packageName, bool lowerCase)
	{
		using var session = new ConsoleCaptureSession();
		await ListAllCommand.ExecuteAsync(packageName, null);

		await Verify(session.Content)
			.UseParameters(packageName, lowerCase);
	}
}