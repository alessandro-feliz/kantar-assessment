using KantarAssessment.ShoppingBasketAPI.Data;
using KantarAssessment.ShoppingBasketAPI.Model;
using KantarAssessment.ShoppingBasketAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KantarAssessment.ShoppingBasketAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ShoppingContext _context;

        public OrderRepository(ShoppingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Order>> GetAll()
        {
            return await _context.Orders.Include(o => o.Items).ToListAsync();
        }

        public async Task SaveAsync(Order order)
        {
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();
        }
    }
}