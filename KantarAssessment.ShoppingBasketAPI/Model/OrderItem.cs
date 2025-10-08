namespace KantarAssessment.ShoppingBasketAPI.Model
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitFinalPrice { get; set; }
        public decimal UnitBasePrice { get; set; }
        public decimal TotalFinalPrice => UnitFinalPrice * Quantity;
        public decimal TotalBasePrice => UnitBasePrice * Quantity;
    }
}