using System.CommandLine;
using Microsoft.Extensions.Logging;
using NugetUnlister.Commands;
using NugetUnlister.Commands.Hierarchy;
using NugetUnlister.IntegrationTests.Toolkit;
using NugetUnlister.Scopes;

namespace NugetUnlister.IntegrationTests;

[UsesVerify]
public class CommandTests
{
	[Theory]
	[InlineData("SymbolSource.TestPackage", false, LogLevel.Warning)]
	[InlineData("SymbolSource.TestPackage", false, LogLevel.Debug)]
	[InlineData("SymbolSource.TestPackage", false, LogLevel.Information)]
	[InlineData("SymbolSource.TestPackage", false, LogLevel.Critical)]
	[InlineData("SymbolSource.TestPackage", false, LogLevel.Error)]
	[InlineData("SymbolSource.TestPackage", false, LogLevel.Trace)]
	[InlineData("symbolsource.testpackage", true, LogLevel.Information)]
	[InlineData("symbolsource.testpackage", true, null)]
	public async Task ListAll(string packageName, bool lowerCase, LogLevel? verbosity)
	{
		using var serviceScope = new ServiceScope();
		using var session = new ConsoleCaptureSession();
		var rootCommand = new ApplicationRootCommand();
		if (verbosity is null)
		{
			await rootCommand.InvokeAsync($"list all {packageName}");
		}
		else
		{
			await rootCommand.InvokeAsync($"list all {packageName} -v {verbosity}");
		}

		await Verify(session.Content)
			.UseParameters(packageName, lowerCase, verbosity);
	}
}