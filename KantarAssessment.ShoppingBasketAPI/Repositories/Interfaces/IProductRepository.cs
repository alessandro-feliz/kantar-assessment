using KantarAssessment.ShoppingBasketAPI.Model;

namespace KantarAssessment.ShoppingBasketAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
    }
}