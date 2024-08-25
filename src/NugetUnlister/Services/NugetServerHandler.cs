using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NugetUnlister.Entities;
using NugetUnlister.Interfaces;
using NugetUnlister.Requests;

namespace NugetUnlister.Services;

internal class NugetServerHandler(
	ILogger logger,
	INugetCli nugetCli,
	IEnumerable<IPackageProvider> packageProviders
) : IRequestHandler
{
	public async Task HandleAsync(ListRequest request)
	{
		var package = await LoadPackageAsync(request.PackageName, request.PackageSource, CancellationToken.None);
		package.Filter(request);

		foreach (var metadata in package.Metadata)
		{
			if (!metadata.Listed)
			{
				logger.LogDebug("{Version} already delisted", metadata.Version);
				continue;
			}

			logger.LogInformation(metadata.Version);
		}
	}

	public async Task HandleAsync(ListAllRequest request)
	{
		var package = await LoadPackageAsync(request.PackageName, request.PackageSource, CancellationToken.None);

		foreach (var metadata in package.Metadata)
		{
			if (!metadata.Listed)
			{
				logger.LogDebug("{Version} already delisted", metadata.Version);
				continue;
			}

			logger.LogInformation(metadata.Version);
		}
	}

	public async Task HandleAsync(DropBeforeRequest request)
	{
		var package = await LoadPackageAsync(request.PackageName, request.PackageSource, CancellationToken.None);
		package.Filter(request);

		foreach (var metadata in package.Metadata)
		{
			if (!metadata.Listed)
			{
				logger.LogDebug("{Version} already delisted", metadata.Version);
				continue;
			}

			await nugetCli.DropAsync(request.PackageName, metadata.Version, request.ApiKey);
		}
	}

	public async Task HandleAsync(DropLikeRequest request)
	{
		var package = await LoadPackageAsync(request.PackageName, request.PackageSource, CancellationToken.None);

		package.Filter(request);

		foreach (var metadata in package.Metadata)
		{
			if (!metadata.Listed)
			{
				logger.LogDebug("{Version} already delisted", metadata.Version);
				continue;
			}

			await nugetCli.DropAsync(request.PackageName, metadata.Version, request.ApiKey);
		}
	}

	private async Task<Package> LoadPackageAsync(string packageName, string source, CancellationToken cancellationToken)
	{
		foreach (var packageProvider in packageProviders.OrderByDescending(d => d.Priority))
		{
			if (await packageProvider.LoadPackageAsync(packageName, cancellationToken) is { } package)
			{
				return package;
			}
			else
			{
				logger.LogDebug("{Provider} is unable to provide a metadata package for the source {Source}", packageProvider.GetType().Name, source);
			}
		}

		throw new ExitCodeException(1, "No package provider was able to provide a metadata package");
	}
}
