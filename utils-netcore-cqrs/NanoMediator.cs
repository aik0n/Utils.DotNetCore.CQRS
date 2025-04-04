using System;
using System.Threading;
using System.Threading.Tasks;

namespace utils_netcore_cqrs
{
    public class NanoMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public NanoMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<TDataResponse> Send<TDataResponse>(IDataRequest<TDataResponse> request, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IDataRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TDataResponse));

            var handler = _serviceProvider.GetService(handlerType) ?? throw new InvalidOperationException($"Handler for '{request.GetType().Name}' not found.");

            return await ((dynamic)handler).Handle((dynamic)request, cancellationToken);
        }
    }
}