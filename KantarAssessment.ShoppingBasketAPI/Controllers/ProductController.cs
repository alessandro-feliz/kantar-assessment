using AutoMapper;
using KantarAssessment.ShoppingBasketAPI.Dto;
using KantarAssessment.ShoppingBasketAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KantarAssessment.ShoppingBasketAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();

            return Ok(_mapper.Map<List<ProductDto>>(products));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductDto>(product));
        }
    }
}