namespace KantarAssessment.ShoppingBasketAPI.Model
{
    public class Promotion
    {
        public int PromotionId { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; } = new Event();
        public Discount Discount { get; set; } = new Discount();
        public PromotionCondition Condition { get; set; } = new PromotionCondition();

        public bool DoConditionsMatch(Basket basket)
        {
            if (!Event.IsActive) return false;

            var productMatching = basket.Items.Where(i => i.Product.ProductId == Condition.RequiredProductId).FirstOrDefault();

            return basket.BasePrice > Condition.MinBasketTotal && productMatching?.Quantity >= Condition.RequiredQuantity;
        }
    }
}