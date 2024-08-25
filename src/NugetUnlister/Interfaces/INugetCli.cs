using System.Threading.Tasks;

namespace NugetUnlister.Interfaces;

public interface INugetCli
{
	Task DropAsync(string packagename, string version, string apiKey);
}
