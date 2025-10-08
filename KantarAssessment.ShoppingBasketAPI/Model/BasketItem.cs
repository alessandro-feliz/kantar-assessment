namespace KantarAssessment.ShoppingBasketAPI.Model
{
    public class BasketItem
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal BasePrice => CalculateBasePrice();
        public decimal FinalPrice => CalculateFinalPrice();

        public BasketItem() { }

        public BasketItem(Product product, int quantity)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));
            ProductId = product.ProductId;
            Quantity = quantity > 0 ? quantity : throw new ArgumentOutOfRangeException(nameof(quantity), "Invalid quantity");
        }

        public void IncreaseQuantity(int additionalQuantity)
        {
            if (additionalQuantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(additionalQuantity), "Quantity must be greater than zero.");

            Quantity += additionalQuantity;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(newQuantity), "Quantity must be greater than zero.");

            Quantity = newQuantity;
        }

        private decimal CalculateBasePrice()
        {
            return Product.BasePrice * Quantity;
        }

        private decimal CalculateFinalPrice()
        {
            return Product.Price * Quantity;
        }
    }
}