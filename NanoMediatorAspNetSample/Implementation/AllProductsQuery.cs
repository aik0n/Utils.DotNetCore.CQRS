using NanoMediatorAspNetSample.Database;
using utils_netcore_cqrs;

namespace NanoMediatorAspNetSample.Implementation
{
    public class AllProductsQuery : IDataRequest<IEnumerable<Product>>
    {
    }
}