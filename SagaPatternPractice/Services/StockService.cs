using SagaPatternPractice.Data;

namespace SagaPatternPractice.Services
{
    public class StockService
    {
        private readonly AppDbContext _context;

        public StockService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ReserveStockAsync(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null) return false;
            if (product.StockQuantity < quantity) return false;

            product.StockQuantity -= quantity;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task UnreserveStockAsync(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null) return;

            product.StockQuantity += quantity;

            await _context.SaveChangesAsync();
        }
    }
}
