using KantarAssessment.ShoppingBasketAPI.Model;

namespace KantarAssessment.ShoppingBasketAPI.Services.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetBasketAsync();
        Task<Basket> AddBasketItemAsync(BasketItem basketItem);
        Task<Basket> UpdateBasketItemAsync(BasketItem basketItem);
        Task<Basket> RemoveBasketItemAsync(BasketItem basketItem);
    }
}