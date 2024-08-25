using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NugetUnlister.Interfaces;

namespace NugetUnlister.Services;

internal class HttpLoader(
	IHttpClientFactory clientFactory,
	ILogger logger
) : IHttpLoader
{
	public async Task<T?> GetFromJsonAsync<T>(string url, CancellationToken cancellationToken)
	{
		var httpClient = clientFactory.CreateClient();
		logger.LogDebug("Loading data from {Url}", url);
		return await httpClient.GetFromJsonAsync<T>(url, cancellationToken);
	}
}
