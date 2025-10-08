using KantarAssessment.ShoppingBasketAPI.Model;
using KantarAssessment.ShoppingBasketAPI.Repositories.Interfaces;
using KantarAssessment.ShoppingBasketAPI.Services.Interfaces;

namespace KantarAssessment.ShoppingBasketAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBasketRepository _basketRepository;

        public OrderService(IOrderRepository orderRepository, IBasketRepository basketRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAll();
        }

        public async Task<Order> CheckoutAsync(Basket basket)
        {
            var order = new Order
            {
                Items = basket.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitBasePrice = i.Product.BasePrice,
                    UnitFinalPrice = i.Product.Price
                }).ToList(),
                BasePrice = basket.BasePrice,
                FinalPrice = basket.FinalPrice,
                OrderDate = DateTime.UtcNow
            };

            await _orderRepository.SaveAsync(order);

            await _basketRepository.ClearBaskeAsync();

            return order;
        }
    }
}