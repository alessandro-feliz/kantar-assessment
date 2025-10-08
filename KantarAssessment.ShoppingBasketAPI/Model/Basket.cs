namespace KantarAssessment.ShoppingBasketAPI.Model
{
    public class Basket
    {
        public int BasketId { get; set; }
        public ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();
        public decimal BasePrice => CalculateBasePrice();
        public decimal FinalPrice => CalculateFinalPrice();

        public Basket() { }

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

            var existingItem = Items.FirstOrDefault(i => i.Product.ProductId == product.ProductId);

            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                Items.Add(new BasketItem(product, quantity));
            }
        }

        public void UpdateItem(Product product, int newQuantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.Product.ProductId == product.ProductId);

            if (existingItem == null)
                throw new InvalidOperationException("Product does not exist in the basket.");

            if (newQuantity <= 0)
            {
                Items.Remove(existingItem);
            }
            else
            {
                existingItem.UpdateQuantity(newQuantity);
            }
        }

        public void RemoveItem(Product product)
        {
            var itemToRemove = Items.FirstOrDefault(i => i.Product.ProductId == product.ProductId);
            if (itemToRemove != null)
            {
                Items.Remove(itemToRemove);
            }
        }

        private decimal CalculateBasePrice()
        {
            return Items.Sum(x => x.BasePrice);
        }
        private decimal CalculateFinalPrice()
        {
            return Items.Sum(x => x.FinalPrice);
        }
    }
}