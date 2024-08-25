using System.Threading;
using System.Threading.Tasks;
using NugetUnlister.Entities;

namespace NugetUnlister.Interfaces;

internal interface IPackageProvider
{
	Task<Package?> LoadPackageAsync(string packageName, CancellationToken cancellationToken);
	
	int Priority { get; }
}
