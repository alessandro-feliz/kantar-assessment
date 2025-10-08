using AutoMapper;
using KantarAssessment.ShoppingBasketAPI.Dto;
using KantarAssessment.ShoppingBasketAPI.Model;
using KantarAssessment.ShoppingBasketAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KantarAssessment.ShoppingBasketAPI.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrderController(IMapper mapper, IOrderService orderService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return Ok(_mapper.Map<List<OrderDto>>(orders));
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutAsync([FromBody] BasketDto basketDto)
        {
            if (basketDto == null || basketDto.Items == null || !basketDto.Items.Any())
                return BadRequest("Basket is empty.");

            var basket = _mapper.Map<Basket>(basketDto);

            var order = await _orderService.CheckoutAsync(basket);

            if (order == null)
                return StatusCode(500, "Failed to complete checkout.");

            var orderDto = _mapper.Map<OrderDto>(order);

            return Ok(orderDto);
        }
    }
}