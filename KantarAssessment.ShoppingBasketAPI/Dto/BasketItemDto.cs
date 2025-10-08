namespace KantarAssessment.ShoppingBasketAPI.Dto
{
    public class BasketItemDto
    {
        public int ProductId { get; set; }
        public ProductDto Product { get; set; } = new ProductDto(); 
        public int Quantity { get; set; }
        public decimal BasePrice { get; set; }
        public decimal FinalPrice { get; set; }
    }
}