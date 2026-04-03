using System.Threading;
using System.Threading.Tasks;

namespace Utils.DotNetCore.CQRS
{
    public interface INanoMediator
    {
        Task<TDataResponse> Send<TDataResponse>(IDataRequest<TDataResponse> request, CancellationToken cancellationToken = default);
    }
}