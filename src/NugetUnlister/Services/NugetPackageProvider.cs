using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NugetUnlister.Entities;
using NugetUnlister.Interfaces;
using NugetUnlister.PackageLoaders.Nuget;

namespace NugetUnlister.Services;

internal class NugetPackageProvider(
	IEnvironmentInformation environmentInformation,
	ILogger logger,
	IHttpLoader httpLoader
) : IPackageProvider
{
	public async Task<Package?> LoadPackageAsync(string packageName, CancellationToken cancellationToken)
	{
		var result = await httpLoader.GetFromJsonAsync<NugetServiceIndex>(environmentInformation.SymbolSource, cancellationToken);
		if (result is null)
			throw new ExitCodeException(1, $"Failed to load service index of {environmentInformation.SymbolSource}");

		List<PackageLoaderBase> loaders = [
			new RegistrationsBaseUrlLoaderThreeSixZero(result, int.MaxValue)
		];
		foreach (var loader in loaders.OrderByDescending(d => d.Priority))
		{
			if (!loader.CanProvide())
			{
				logger.LogDebug("{Loader} cannot handle this service index", loader.GetType().Name);
				continue;
			}

			if (await loader.LoadAsync(httpLoader, packageName, cancellationToken) is not { } package)
				throw new ExitCodeException(2, $"{loader.GetType().Name} is supposed to provide data, but failed to do so");
			
			return package;
		}

		return null;
	}

	public int Priority => int.MaxValue;
}
