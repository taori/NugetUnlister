using System.Net;
using System.Net.Http;
using Amusoft.Toolkit.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NugetUnlister.Interfaces;
using NugetUnlister.Services;

namespace NugetUnlister.Scopes;

internal class ServiceScope : AmbientScope<ServiceScope>
{
	public ServiceScope()
	{
		var collection = new ServiceCollection();
		AddServices(collection);
		ServiceProvider = collection.BuildServiceProvider(true);
	}

	private void AddServices(ServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IEnvironmentInformation, EnvironmentInformation>();
		serviceCollection.AddSingleton<IApiCalls, ApiCalls>();
		serviceCollection.AddSingleton<IRequestHandler, NugetServerHandler>();
		serviceCollection.AddSingleton<IPackageProvider, GitlabPackageProvider>();
		serviceCollection.AddSingleton<IPackageProvider, NugetPackageProvider>();
		serviceCollection.AddSingleton<ILogger, ConsoleLogger>();
		serviceCollection.AddSingleton<INugetCli, NugetCli>();
		serviceCollection.AddSingleton<IHttpLoader, HttpLoader>();
		serviceCollection.AddHttpClient().ConfigureHttpClientDefaults(d => d.ConfigurePrimaryHttpMessageHandler(handler => new HttpClientHandler()
				{
					AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.Brotli
				}
			)
		);
	}

	public ServiceProvider ServiceProvider { get; set; }
}
