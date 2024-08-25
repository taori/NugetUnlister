using System.Threading;
using System.Threading.Tasks;
using NugetUnlister.Entities;
using NugetUnlister.Interfaces;

namespace NugetUnlister.Services;

internal class GitlabPackageProvider : IPackageProvider
{
	public Task<Package?> LoadPackageAsync(string packageName, CancellationToken cancellationToken)
	{
		return Task.FromResult(default(Package?));
	}

	public int Priority { get; }
}
