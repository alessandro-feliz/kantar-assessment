namespace KantarAssessment.ShoppingBasketAPI.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Price { get { return CalculatePriceWithDiscount(); } }
        public string? ImageUrl { get; set; }
        public ICollection<Discount> Discounts { get; set; } = new List<Discount>();
        public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        private decimal CalculatePriceWithDiscount()
        {
            var price = BasePrice;

            foreach (var discount in Discounts)
            {
                if (discount.IsValid)
                {
                    price = discount.Apply(price);
                }
            }

            return price;
        }
    }
}