using System.Threading.Tasks;
using NugetUnlister.Requests;

namespace NugetUnlister.Interfaces;

internal interface IRequestHandler
{
	Task HandleAsync(ListRequest request);
	Task HandleAsync(ListAllRequest request);
	Task HandleAsync(DropBeforeRequest request);
	Task HandleAsync(DropLikeRequest request);
}
