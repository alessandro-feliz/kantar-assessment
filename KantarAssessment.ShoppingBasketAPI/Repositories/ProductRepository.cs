using KantarAssessment.ShoppingBasketAPI.Data;
using KantarAssessment.ShoppingBasketAPI.Model;
using KantarAssessment.ShoppingBasketAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KantarAssessment.ShoppingBasketAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShoppingContext _context;

        public ProductRepository(ShoppingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Discounts).ThenInclude(d => d.Event)
                .Include(p => p.Promotions).ThenInclude(pr => pr.Event)
                .Include(p => p.Promotions).ThenInclude(pr => pr.Discount)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Discounts).ThenInclude(d => d.Event)
                .Include(p => p.Promotions).ThenInclude(pr => pr.Event)
                .Include(p => p.Promotions).ThenInclude(pr => pr.Discount)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
    }
}