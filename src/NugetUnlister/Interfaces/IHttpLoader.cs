using System.Threading;
using System.Threading.Tasks;

namespace NugetUnlister.Interfaces;

public interface IHttpLoader
{
	Task<T?> GetFromJsonAsync<T>(string url, CancellationToken cancellationToken);
}
