#nullable enable
using NugetUnlister.Scopes;
using Shouldly;
using Xunit;

namespace NugetUnlister.UnitTests.Tests;

public class SecretScopeTests
{
	[Theory]
	[InlineData("hello there mario", "santa", "hello there mario")]
	[InlineData("hello there santa", "santa", "hello there ***")]
	[InlineData("hello there santa", null, "hello there santa")]
	public void NoSecretsNoChange(string input, string? secret, string expected)
	{
		var scope = new SecretsScope();
		if (secret is not null)
			scope.WithSecret(secret);
		
		scope.GetScrubbed(input).ShouldBe(expected);
	}
}