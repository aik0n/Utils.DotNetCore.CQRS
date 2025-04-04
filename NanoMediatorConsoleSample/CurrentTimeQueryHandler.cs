using utils_netcore_cqrs;

namespace NanoMediatorConsoleSample
{
    public class CurrentTimeQueryHandler : IDataRequestHandler<CurrentTimeQuery, string>
    {
        public Task<string> Handle(CurrentTimeQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult($"Current UTC Time: {DateTime.UtcNow:HH:mm:ss}");
        }
    }
}