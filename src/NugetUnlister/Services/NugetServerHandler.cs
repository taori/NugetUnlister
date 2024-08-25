using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
		foreach (var packageProvider in packageProviders.OrderByDescending(d => d.Priority))
		{
			if(await packageProvider.LoadPackageAsync(request.PackageName, CancellationToken.None) is not {} package)
				continue;

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

			return;
		}
		
		logger.LogCritical("No package provider was able to handle the request");
	}

	public async Task HandleAsync(ListAllRequest request)
	{
		foreach (var packageProvider in packageProviders.OrderByDescending(d => d.Priority))
		{
			if(await packageProvider.LoadPackageAsync(request.PackageName, CancellationToken.None) is not {} package)
				continue;

			foreach (var metadata in package.Metadata)
			{
				if (!metadata.Listed)
				{
					logger.LogDebug("{Version} already delisted", metadata.Version);
					continue;
				}
				
				logger.LogInformation(metadata.Version);
			}

			return;
		}
		
		logger.LogCritical("No package provider was able to handle the request");
	}

	public async Task HandleAsync(DropBeforeRequest request)
	{
		foreach (var packageProvider in packageProviders.OrderByDescending(d => d.Priority))
		{
			if(await packageProvider.LoadPackageAsync(request.PackageName, CancellationToken.None) is not {} package)
				continue;

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

			return;
		}
		
		logger.LogCritical("No package provider was able to handle the request");
	}

	public async Task HandleAsync(DropLikeRequest request)
	{
		foreach (var packageProvider in packageProviders.OrderByDescending(d => d.Priority))
		{
			if(await packageProvider.LoadPackageAsync(request.PackageName, CancellationToken.None) is not {} package)
				continue;

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

			return;
		}
		
		logger.LogCritical("No package provider was able to handle the request");
	}
}
