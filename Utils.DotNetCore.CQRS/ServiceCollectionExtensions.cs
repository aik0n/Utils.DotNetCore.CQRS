using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace utils_netcore_cqrs
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNanoMediator(this IServiceCollection services, Assembly assembly)
        {
            services.AddScoped<INanoMediator, NanoMediator>();

            var handlerInterfaceType = typeof(IDataRequestHandler<,>);

            var handlerTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Select(t => new
                {
                    Type = t,
                    Interfaces = t.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
                })
                .Where(x => x.Interfaces.Any());

            foreach (var handler in handlerTypes)
            {
                foreach (var handlerInterface in handler.Interfaces)
                {
                    services.AddScoped(handlerInterface, handler.Type);
                }
            }

            return services;
        }
    }
}