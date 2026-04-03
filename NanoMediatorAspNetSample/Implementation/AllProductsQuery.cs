using NanoMediatorAspNetSample.Database;
using Utils.DotNetCore.CQRS;

namespace NanoMediatorAspNetSample.Implementation
{
    public class AllProductsQuery : IDataRequest<IEnumerable<Product>>
    {
    }
}