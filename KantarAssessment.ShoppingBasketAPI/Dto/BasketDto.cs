namespace KantarAssessment.ShoppingBasketAPI.Dto
{
    public class BasketDto
    {
        public int BasketId { get; set; }
        public ICollection<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
        public decimal BasePrice { get; set; }
        public decimal FinalPrice { get; set; }
    }
}