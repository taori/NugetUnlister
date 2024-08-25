using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NugetUnlister.Interfaces;
using NugetUnlister.Requests;
using NugetUnlister.Scopes;

namespace NugetUnlister.Services;

internal class ApiCalls : IApiCalls
{
	private readonly IEnvironmentInformation _environmentInformation;
	private readonly IRequestHandler _requestHandler;

	public ApiCalls(IEnvironmentInformation environmentInformation, IRequestHandler requestHandler)
	{
		_environmentInformation = environmentInformation;
		_requestHandler = requestHandler;
	}
	
	public Task ListAsync(LogLevel verbosity, string packageName, string? version, string? pattern, bool? prerelease, string packageSource)
	{
		using var scope = new LogLevelScope(verbosity);
		_environmentInformation.SymbolSource = packageSource;
		return _requestHandler.HandleAsync(new ListRequest(packageName, version, pattern, prerelease, packageSource));
	}

	public Task ListAllAsync(LogLevel logLevel, string packageName, string packageSource)
	{
		using var scope = new LogLevelScope(logLevel);
		_environmentInformation.SymbolSource = packageSource;
		return _requestHandler.HandleAsync(new ListAllRequest(packageName, packageSource));
	}

	public Task DropBeforeAsync(LogLevel logLevel, string packageName, string version, string apiKey, string packageSource, bool? prerelease)
	{
		using var scope = new LogLevelScope(logLevel);
		using (new SecretsScope().WithSecret(apiKey))
		{
			_environmentInformation.SymbolSource = packageSource;
			return _requestHandler.HandleAsync(new DropBeforeRequest(packageName, version, apiKey, packageSource, prerelease));
		}
	}

	public Task DropLikeAsync(LogLevel logLevel, string packageName, string pattern, string apiKey, string packageSource, bool? prerelease)
	{
		using var scope = new LogLevelScope(logLevel);
		using (new SecretsScope().WithSecret(apiKey))
		{
			_environmentInformation.SymbolSource = packageSource;
			return _requestHandler.HandleAsync(new DropLikeRequest(packageName, pattern, apiKey, packageSource, prerelease));
		}
	}
}
