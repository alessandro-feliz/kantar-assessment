using KantarAssessment.ShoppingBasketAPI.Data;
using KantarAssessment.ShoppingBasketAPI.Model;
using KantarAssessment.ShoppingBasketAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KantarAssessment.ShoppingBasketAPI.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ShoppingContext _context;

        public EventRepository(ShoppingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events.ToListAsync();
        }
    }
}