using AutoMapper;
using KantarAssessment.ShoppingBasketAPI.Dto;
using KantarAssessment.ShoppingBasketAPI.Model;
using KantarAssessment.ShoppingBasketAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KantarAssessment.ShoppingBasketAPI.Controllers
{
    [ApiController]
    [Route("api/basket")]
    public class BasketController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;

        public BasketController(IMapper mapper, IProductService productService, IBasketService basketService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketAsync()
        {
            var basket = await _basketService.GetBasketAsync();

            return Ok(_mapper.Map<BasketDto>(basket));
        }

        [HttpPost]
        public async Task<IActionResult> AddBasketItemAsync([FromBody] BasketItemDto basketItemDto)
        {
            if (basketItemDto == null)
                return BadRequest("Invalid basket item.");

            var product = await _productService.GetProductByIdAsync(basketItemDto.ProductId);
            if (product == null)
                return NotFound($"Product with ID {basketItemDto.ProductId} not found.");

            var basketItem = new BasketItem(product, basketItemDto.Quantity);

            var basket = await _basketService.AddBasketItemAsync(basketItem);

            return Ok(_mapper.Map<BasketDto>(basket));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBasketItemAsync([FromBody] BasketItemDto basketItemDto)
        {
            if (basketItemDto == null)
                return BadRequest("Invalid basket item.");
            
            var product = await _productService.GetProductByIdAsync(basketItemDto.ProductId);
            if (product == null)
                return NotFound($"Product with ID {basketItemDto.ProductId} not found.");

            var basketItem = new BasketItem(product, basketItemDto.Quantity);

            var basket = await _basketService.UpdateBasketItemAsync(basketItem);

            return Ok(_mapper.Map<BasketDto>(basket));
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveBasketItemAsync([FromBody] BasketItemDto basketItemDto)
        {
            if (basketItemDto == null)
                return BadRequest("Invalid basket item.");

            var product = await _productService.GetProductByIdAsync(basketItemDto.ProductId);
            if (product == null)
                return NotFound($"Product with ID {basketItemDto.ProductId} not found.");

            var basketItem = new BasketItem(product, 1);

            var basket = await _basketService.RemoveBasketItemAsync(basketItem);

            return Ok(_mapper.Map<BasketDto>(basket));
        }
    }
}