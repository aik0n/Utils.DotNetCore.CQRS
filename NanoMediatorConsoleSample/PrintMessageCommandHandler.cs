using utils_netcore_cqrs;

namespace NanoMediatorConsoleSample
{
    public class PrintMessageCommandHandler : IDataRequestHandler<PrintMessageCommand, bool>
    {
        public Task<bool> Handle(PrintMessageCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Command Received] {request.Message}");
            return Task.FromResult(true);
        }
    }
}