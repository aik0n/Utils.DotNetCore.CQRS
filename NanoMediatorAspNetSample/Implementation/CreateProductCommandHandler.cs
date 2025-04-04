using NanoMediatorAspNetSample.Database;
using utils_netcore_cqrs;

namespace NanoMediatorAspNetSample.Implementation
{
    public class CreateProductCommandHandler : IDataRequestHandler<CreateProductCommand, Product>
    {
        private readonly ApplicationContext _context;

        public CreateProductCommandHandler(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price
            };

            try
            {
                await _context.Products.AddAsync(product, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return product;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}