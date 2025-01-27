using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NugetUnlister.Entities;
using NugetUnlister.Interfaces;

namespace NugetUnlister.PackageLoaders.Nuget;

internal class RegistrationsBaseUrlLoaderThreeSixZero : PackageLoaderBase
{
	public RegistrationsBaseUrlLoaderThreeSixZero(NugetServiceIndex serviceIndex, int priority) : base(serviceIndex, priority)
	{
		this.ServiceIdsByType = ServiceIndex.Resources.ToLookup(d => d.Type.ToLowerInvariant(), d => d.Id);
	}

	public ILookup<string,string> ServiceIdsByType { get; }

	public override bool CanProvide()
	{
		return ServiceIdsByType["RegistrationsBaseUrl/3.6.0".ToLowerInvariant()].Any();
	}

	public override async Task<Package?> LoadAsync(IHttpLoader httpLoader, string packageName, CancellationToken cancellationToken)
	{
		if(ServiceIdsByType["RegistrationsBaseUrl/3.6.0".ToLowerInvariant()].FirstOrDefault() is not {} registration)
			throw new ExitCodeException(2, $"Failed to find RegistrationsBaseUrl/3.6.0");

		var resourceUrl = $"{registration}{packageName.ToLowerInvariant()}/index.json";
		var registrationContent = await httpLoader.GetFromJsonAsync<CatalogRootThreeSixZero>(resourceUrl, cancellationToken);
		if (registrationContent is null)
			throw new ExitCodeException(3, $"Failed to find RegistrationsBaseUrl/3.6.0");
		
		/*
		 * https://learn.microsoft.com/en-us/nuget/api/registration-base-url-resource
		 * this case can happen for packages where there is a larger number of package data. In this case individual packages have to be queried using their version number.
		 * Reported through https://github.com/taori/NugetUnlister/issues/36
		 */
		foreach (var page in registrationContent.catalogPages)
		{
			if (page.packages is null)
			{
				var remotePage = await httpLoader.GetFromJsonAsync<CatalogRootThreeSixZero.CatalogPage>(page.Id, cancellationToken);
				page.packages = remotePage?.packages;
				
				if (page.packages is null)
					throw new ExitCodeException(4, $"Failed to load {page.Id} as CatalogPage");
			}
		}
		
		return new Package()
		{
			Metadata = registrationContent.catalogPages
				.SelectMany(page => page.packages?.Select(package => package.details))
				.Select(detail => new PackageMetadata(detail.version, detail.listed)).ToArray()
		};
	}
}
