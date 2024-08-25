using System.Threading;
using System.Threading.Tasks;
using NugetUnlister.Entities;
using NugetUnlister.Interfaces;

namespace NugetUnlister.PackageLoaders.Nuget;

internal abstract class PackageLoaderBase
{
	protected PackageLoaderBase(NugetServiceIndex serviceIndex, int priority)
	{
		ServiceIndex = serviceIndex;
		Priority = priority;
	}

	public NugetServiceIndex ServiceIndex { get; }
	
	public int Priority { get; }

	public abstract bool CanProvide();
	
	public abstract Task<Package?> LoadAsync(IHttpLoader httpLoader, string packageName, CancellationToken cancellationToken);
}
