using Utils.DotNetCore.CQRS;

namespace NanoMediatorConsoleSample
{
    public class PrintMessageCommand : IDataRequest<bool>
    {
        public string Message { get; }

        public PrintMessageCommand(string message)
        {
            Message = message;
        }
    }
}