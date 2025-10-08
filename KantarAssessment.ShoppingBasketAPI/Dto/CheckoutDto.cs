namespace KantarAssessment.ShoppingBasketAPI.Dto
{
    public class CheckoutDto
    {
        public List<BasketItemDto> Items { get; set; } = new();
    }
}