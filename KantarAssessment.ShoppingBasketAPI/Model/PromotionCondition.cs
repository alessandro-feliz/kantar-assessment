namespace KantarAssessment.ShoppingBasketAPI.Model
{
    public class PromotionCondition
    {
        public int RequiredProductId { get; set; }
        public int RequiredQuantity { get; set; } = 0;
        public decimal MinBasketTotal { get; set; } = 0;
    }
}