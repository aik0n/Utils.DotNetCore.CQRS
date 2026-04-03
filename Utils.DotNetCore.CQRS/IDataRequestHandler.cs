using System.Threading;
using System.Threading.Tasks;

namespace Utils.DotNetCore.CQRS
{
    public interface IDataRequestHandler<TDataRequest, TDataResponse> where TDataRequest : IDataRequest<TDataResponse>
    {
        Task<TDataResponse> Handle(TDataRequest request, CancellationToken cancellationToken);
    }
}