using NugetUnlister.Interfaces;

namespace NugetUnlister.Services;

internal class EnvironmentInformation : IEnvironmentInformation
{
	private string _symbolSource = "https://api.nuget.org/v3/index.json";

	public string SymbolSource
	{
		get => _symbolSource;
		set => _symbolSource = value;
	}
}
