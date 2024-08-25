using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NugetUnlister.Entities;
using NugetUnlister.Interfaces;
using NugetUnlister.Services;
using NugetUnlister.UnitTests.Utilities;
using Shouldly;
using Xunit;

namespace NugetUnlister.UnitTests.Tests;

public class PackageProviderTests
{
	[Fact(Timeout = 15_000)]
	public async Task NugetPackageProvider()
	{
		var embeddedResourceReader = new EmbeddedResourceReader(typeof(PackageProviderTests).Assembly);
		
		var logger = new Mock<ILogger>();
		var environment = new Mock<IEnvironmentInformation>();
		environment.Setup(d => d.SymbolSource).Returns("https://api.nuget.org/v3/index.json");
		var serviceIndexLoader = new Mock<IHttpLoader>();
		serviceIndexLoader.Setup(d => d.GetFromJsonAsync<NugetServiceIndex>(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(JsonSerializer.Deserialize<NugetServiceIndex>(embeddedResourceReader.GetContent("TestContent.input.nuget-serviceindex.json"))));
		serviceIndexLoader.Setup(d => d.GetFromJsonAsync<RegistrationBaseReplyThreeSixZero>(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(JsonSerializer.Deserialize<RegistrationBaseReplyThreeSixZero>(embeddedResourceReader.GetContent("TestContent.input.semver-response-amusoft.toolkit.threading.json"))));
		var provider = new NugetPackageProvider(environment.Object, logger.Object, serviceIndexLoader.Object);
		var package = await provider.LoadPackageAsync("Amusoft.Toolkit.Threading", CancellationToken.None);
		package.ShouldNotBeNull();
	}
}