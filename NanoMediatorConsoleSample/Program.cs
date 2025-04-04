using Microsoft.Extensions.DependencyInjection;
using utils_netcore_cqrs;

namespace NanoMediatorConsoleSample
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddNanoMediator(typeof(Program).Assembly)
                .BuildServiceProvider();

            var mediator = serviceProvider.GetRequiredService<NanoMediator>();

            var timeResult = await mediator.Send(new CurrentTimeQuery());
            Console.WriteLine($"[Query Result] {timeResult}");

            await mediator.Send(new PrintMessageCommand("Hello from Custom Mediator!"));
            Console.WriteLine("\nPress any key to exit...");

            Console.ReadKey();
        }
    }
}