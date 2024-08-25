using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NugetUnlister.Interfaces;

internal interface IApiCalls
{
	Task ListAsync(LogLevel verbosity, string packageName, string? version, string? pattern, bool? prerelease, string packageSource);
	Task ListAllAsync(LogLevel logLevel, string packageName, string packageSource);
	Task DropBeforeAsync(LogLevel logLevel, string packageName, string version, string apiKey, string packageSource, bool? prerelease);
	Task DropLikeAsync(LogLevel logLevel, string packageName, string pattern, string apiKey, string packageSource, bool? prerelease);
}
