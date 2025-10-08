using KantarAssessment.ShoppingBasketAPI.Model;

namespace KantarAssessment.ShoppingBasketAPI.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<Basket> GetBasketAsync();
        Task<Basket> ClearBaskeAsync();
        Task<Basket> AddBasketItemAsync(BasketItem basketItem);
        Task<Basket> UpdateBasketItemAsync(BasketItem basketItem);
        Task<Basket> RemoveBasketItemAsync(BasketItem basketItem);
    }
}