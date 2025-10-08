namespace KantarAssessment.ShoppingBasketAPI.Dto
{
    public class PromotionDto
    {
        public int PromotionId { get; set; }
        public EventDto Event { get; set; } = new EventDto();
        public DiscountDto Discount { get; set; } = new DiscountDto();
        public bool ApplyOncePerBasket { get; set; }
        public PromotionConditionDto Condition { get; set; } = new PromotionConditionDto();
    }
}