namespace KantarAssessment.ShoppingBasketAPI.Dto
{
    public class PromotionConditionDto
    {
        public int? RequiredProductId { get; set; }
        public int? RequiredQuantity { get; set; }
        public decimal? MinBasketTotal { get; set; }
    }
}