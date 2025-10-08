namespace KantarAssessment.ShoppingBasketAPI.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public decimal FinalPrice { get; set; }
        public decimal BasePrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}