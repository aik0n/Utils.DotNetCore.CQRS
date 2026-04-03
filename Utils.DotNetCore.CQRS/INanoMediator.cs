using System.Threading;
using System.Threading.Tasks;

namespace utils_netcore_cqrs
{
    public interface INanoMediator
    {
        Task<TDataResponse> Send<TDataResponse>(IDataRequest<TDataResponse> request, CancellationToken cancellationToken = default);
    }
}