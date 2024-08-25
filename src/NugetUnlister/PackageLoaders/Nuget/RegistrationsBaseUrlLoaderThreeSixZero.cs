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
		
		var registrationContent = await httpLoader.GetFromJsonAsync<RegistrationBaseReplyThreeSixZero>($"{registration}{packageName.ToLowerInvariant()}/index.json", cancellationToken);
		if (registrationContent is null)
			throw new ExitCodeException(3, $"Failed to find RegistrationsBaseUrl/3.6.0");

		var entries = registrationContent.items.SelectMany(d => d.items.Select(f => f.catalogEntry));
		return new Package()
		{
			Metadata = entries.Select(catalog => new PackageMetadata(catalog.version, catalog.listed)).ToArray()
		};
	}
}
