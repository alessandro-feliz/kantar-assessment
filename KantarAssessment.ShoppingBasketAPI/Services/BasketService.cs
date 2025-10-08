using KantarAssessment.ShoppingBasketAPI.Model;
using KantarAssessment.ShoppingBasketAPI.Repositories.Interfaces;
using KantarAssessment.ShoppingBasketAPI.Services.Interfaces;

namespace KantarAssessment.ShoppingBasketAPI.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;

        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        }

        public async Task<Basket> GetBasketAsync()
        {
            var basket = await _basketRepository.GetBasketAsync();

            CheckPromotions(basket);

            return basket;
        }

        public async Task<Basket> AddBasketItemAsync(BasketItem basketItem)
        {
            var basket = await _basketRepository.AddBasketItemAsync(basketItem);

            CheckPromotions(basket);

            return basket;
        }

        public async Task<Basket> UpdateBasketItemAsync(BasketItem basketItem)
        {
            var basket = await _basketRepository.UpdateBasketItemAsync(basketItem);

            CheckPromotions(basket);

            return basket;
        }

        public async Task<Basket> RemoveBasketItemAsync(BasketItem basketItem)
        {
            var basket = await _basketRepository.RemoveBasketItemAsync(basketItem);

            CheckPromotions(basket);

            return basket;
        }

        private void CheckPromotions(Basket basket)
        {
            var basketItemsWithPromotions = basket.Items.Where(i => i.Product.Promotions != null && i.Product.Promotions.Any()).Distinct();

            foreach (var basketItemWithPromotions in basketItemsWithPromotions)
            {
                foreach (var promotion in basketItemWithPromotions.Product.Promotions)
                {
                    if (promotion.DoConditionsMatch(basket) && promotion.Event.IsActive)
                    {
                        basketItemWithPromotions.Product.Discounts.Add(promotion.Discount);
                    }
                }
            }
        }
    }
}