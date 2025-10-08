namespace KantarAssessment.ShoppingBasketAPI.Model
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public int? EventId { get; set; }
        public Event? Event { get; set; }
        public bool IsValid { get { return Event != null ? Event.IsActive : false; } }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }

        internal decimal Apply(decimal price)
        {
            if (IsValid)
            {
                switch (DiscountType)
                {
                    case DiscountType.FixedAmount:
                        price = price - DiscountValue;
                        break;
                    case DiscountType.Percentage:
                        price = price - price * DiscountValue / 100;
                        break;
                    default:
                        throw new NotImplementedException($"DiscountType {DiscountType} not implemented");
                }
            }

            return price < 0 ? 0 : price;
        }
    }
}