using NanoMediatorAspNetSample.Database;
using utils_netcore_cqrs;

namespace NanoMediatorAspNetSample.Implementation
{
    public class CreateProductCommand : IDataRequest<Product>
    {
        public string Name { get; }

        public decimal Price { get; }

        public CreateProductCommand(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }
}