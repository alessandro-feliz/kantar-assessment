using KantarAssessment.ShoppingBasketAPI.Model;

namespace KantarAssessment.ShoppingBasketAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
    }
}