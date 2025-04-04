using System.Threading;
using System.Threading.Tasks;

namespace utils_netcore_cqrs
{
    public interface IDataRequestHandler<TDataRequest, TDataResponse> where TDataRequest : IDataRequest<TDataResponse>
    {
        Task<TDataResponse> Handle(TDataRequest request, CancellationToken cancellationToken);
    }
}