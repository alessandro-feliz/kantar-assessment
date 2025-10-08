using KantarAssessment.ShoppingBasketAPI.Model;

namespace KantarAssessment.ShoppingBasketAPI.Dto
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal BasePrice { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Discount> Discounts { get; set; } = new List<Discount>();
        public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
    }
}