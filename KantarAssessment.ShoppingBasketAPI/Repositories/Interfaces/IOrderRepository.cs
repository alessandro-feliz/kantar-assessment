using KantarAssessment.ShoppingBasketAPI.Model;

namespace KantarAssessment.ShoppingBasketAPI.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task SaveAsync(Order order);
        Task<List<Order>> GetAll();
    }
}