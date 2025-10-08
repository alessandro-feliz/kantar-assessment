namespace KantarAssessment.ShoppingBasketAPI.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public IEnumerable<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal FinalPrice { get; set; }
        public decimal BasePrice { get; set; }
    }
}