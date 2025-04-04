using Microsoft.EntityFrameworkCore;
using NanoMediatorAspNetSample.Database;
using utils_netcore_cqrs;

namespace NanoMediatorAspNetSample.Implementation
{
    public class AllProductsQueryHandler : IDataRequestHandler<AllProductsQuery, IEnumerable<Product>>
    {
        private readonly ApplicationContext _context;

        public AllProductsQueryHandler(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> Handle(AllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }
    }
}