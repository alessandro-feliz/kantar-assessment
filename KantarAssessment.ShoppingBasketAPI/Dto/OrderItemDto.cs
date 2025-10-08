namespace KantarAssessment.ShoppingBasketAPI.Dto
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitFinalPrice { get; set; }
        public decimal UnitBasePrice { get; set; }
        public decimal TotalFinalPrice { get; set; }
        public decimal TotalBasePrice { get; set; }
    }
}