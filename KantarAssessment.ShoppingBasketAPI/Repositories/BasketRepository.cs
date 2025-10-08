using KantarAssessment.ShoppingBasketAPI.Data;
using KantarAssessment.ShoppingBasketAPI.Model;
using KantarAssessment.ShoppingBasketAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KantarAssessment.ShoppingBasketAPI.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly ShoppingContext _context;

        public BasketRepository(ShoppingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Basket> GetBasketAsync()
        {
            var basket = await _context.Baskets
          .Include(b => b.Items)
              .ThenInclude(bi => bi.Product)
                  .ThenInclude(p => p.Discounts)
                      .ThenInclude(d => d.Event)
          .Include(b => b.Items)
              .ThenInclude(bi => bi.Product)
                  .ThenInclude(p => p.Promotions)
                      .ThenInclude(promo => promo.Discount)
                          .ThenInclude(d => d.Event)
          .Include(b => b.Items)
              .ThenInclude(bi => bi.Product)
                  .ThenInclude(p => p.Promotions)
                      .ThenInclude(promo => promo.Event) 
          .FirstOrDefaultAsync();

            if (basket == null)
            {
                basket = new Basket
                {
                    Items = new List<BasketItem>()
                };

                _context.Baskets.Add(basket);

                await _context.SaveChangesAsync();
            }

            return basket;
        }

        public async Task<Basket> AddBasketItemAsync(BasketItem basketItem)
        {
            var basket = await GetBasketAsync();

            var existingItem = basket.Items.FirstOrDefault(i => i.ProductId == basketItem.ProductId);

            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(basketItem.Quantity);
                _context.BasketItems.Update(existingItem);
            }
            else
            {
                basketItem.BasketId = basket.BasketId;
                _context.BasketItems.Add(basketItem);
            }

            await _context.SaveChangesAsync();

            return await GetBasketAsync();
        }

        public async Task<Basket> UpdateBasketItemAsync(BasketItem basketItem)
        {
            var basket = await GetBasketAsync();

            var existingItem = basket.Items.FirstOrDefault(i => i.ProductId == basketItem.ProductId);

            if (existingItem != null)
            {
                existingItem.UpdateQuantity(basketItem.Quantity);

                _context.BasketItems.Update(existingItem);

                await _context.SaveChangesAsync();
            }

            return await GetBasketAsync();
        }

        public async Task<Basket> RemoveBasketItemAsync(BasketItem basketItem)
        {
            var basket = await GetBasketAsync();

            var existingItem = basket.Items.FirstOrDefault(i => i.ProductId == basketItem.ProductId);

            if (existingItem != null)
            {
                _context.BasketItems.Remove(existingItem);

                await _context.SaveChangesAsync();
            }

            return await GetBasketAsync();
        }

        public async Task<Basket> ClearBaskeAsync()
        {
            var basket = await GetBasketAsync();

            basket.Items.Clear();

            await _context.SaveChangesAsync();

            return await GetBasketAsync();
        }
    }
}