using KantarAssessment.ShoppingBasketAPI.Model;
using KantarAssessment.ShoppingBasketAPI.Repositories.Interfaces;
using KantarAssessment.ShoppingBasketAPI.Services;
using Moq;

namespace KantarAssessment.ShoppingBasketAPI.Tests
{
    public class BasketServiceTests
    {
        private readonly Mock<IBasketRepository> _mockRepo;
        private readonly BasketService _service;

        public BasketServiceTests()
        {
            _mockRepo = new Mock<IBasketRepository>();
            _service = new BasketService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetBasketAsync_WithValidPercentageDiscount_AppliesDiscountToFinalPrice()
        {
            var soup = new Product
            {
                ProductId = 1,
                Name = "Soup",
                BasePrice = 0.65m,
                Discounts = new List<Discount>(),
                Promotions = new List<Promotion>()
            };

            var apples = new Product
            {
                ProductId = 2,
                Name = "Apples",
                BasePrice = 1.00m,
                Discounts = new List<Discount>
                {
                    new Discount
                    {
                        DiscountId = 1,
                        DiscountType = DiscountType.Percentage,
                        DiscountValue = 10,
                        Event = new Event
                        {
                            StartDate = DateTime.MinValue,
                            EndDate = DateTime.MaxValue
                        }
                    }
                },
                Promotions = new List<Promotion>()
            };

            var basketItems = new List<BasketItem>
            {
                new BasketItem { Product = soup, ProductId = soup.ProductId, Quantity = 2 },
                new BasketItem { Product = apples, ProductId = apples.ProductId, Quantity = 1 }
            };

            var basket = new Basket
            {
                Items = basketItems
            };

            _mockRepo.Setup(r => r.GetBasketAsync()).ReturnsAsync(basket);

            var result = await _service.GetBasketAsync();

            Assert.Equal(2.20m, result.FinalPrice);
        }

        [Fact]
        public async Task GetBasketAsync_WithExpiredDiscount_DoesNotApplyDiscount()
        {
            var soup = new Product
            {
                ProductId = 1,
                Name = "Soup",
                BasePrice = 0.65m,
                Discounts = new List<Discount>(),
                Promotions = new List<Promotion>()
            };

            var apples = new Product
            {
                ProductId = 2,
                Name = "Apples",
                BasePrice = 1.00m,
                Discounts = new List<Discount>
                {
                    new Discount
                    {
                        DiscountId = 1,
                        DiscountType = DiscountType.Percentage,
                        DiscountValue = 10,
                        Event = new Event
                        {
                            StartDate = DateTime.MinValue,
                            EndDate = DateTime.MinValue
                        }
                    }
                },
                Promotions = new List<Promotion>()
            };

            var basketItems = new List<BasketItem>
            {
                new BasketItem { Product = soup, ProductId = soup.ProductId, Quantity = 2 },
                new BasketItem { Product = apples, ProductId = apples.ProductId, Quantity = 1 }
            };

            var basket = new Basket
            {
                Items = basketItems
            };

            _mockRepo.Setup(r => r.GetBasketAsync()).ReturnsAsync(basket);

            var result = await _service.GetBasketAsync();

            Assert.Equal(2.30m, result.FinalPrice);
        }

        [Fact]
        public async Task GetBasketAsync_WithValidMultiBuyPromotion_AppliesDiscountToFinalPrice()
        {
            var soup = new Product
            {
                ProductId = 1,
                Name = "Soup",
                BasePrice = 0.65m,
                Discounts = new List<Discount>(),
                Promotions = new List<Promotion>()
            };

            var bread = new Product
            {
                ProductId = 2,
                Name = "Bread",
                BasePrice = 0.80m,
                Discounts = new List<Discount>(),
                Promotions = new List<Promotion>
                {
                    new Promotion()
                    {
                        PromotionId = 1,
                        Event = new Event
                        {
                            StartDate = DateTime.MinValue,
                            EndDate = DateTime.MaxValue
                        },
                        Discount =  new Discount
                        {
                            DiscountId = 1,
                            DiscountType = DiscountType.Percentage,
                            DiscountValue = 50,
                            Event = new Event
                            {
                                StartDate = DateTime.MinValue,
                                EndDate = DateTime.MaxValue
                            }
                        },
                        Condition = new PromotionCondition()
                        {
                            RequiredProductId = 1,
                            RequiredQuantity = 2
                        }
                    }
                }
            };

            var basketItems = new List<BasketItem>
            {
                new BasketItem { Product = soup, ProductId = soup.ProductId, Quantity = 2 },
                new BasketItem { Product = bread, ProductId = bread.ProductId, Quantity = 1 }
            };

            var basket = new Basket
            {
                Items = basketItems
            };

            _mockRepo.Setup(r => r.GetBasketAsync()).ReturnsAsync(basket);

            var result = await _service.GetBasketAsync();

            Assert.Equal(1.70m, result.FinalPrice);
        }

        [Fact]
        public async Task GetBasketAsync_WithInvalidRequiredQuantityForMultiBuyPromotion_DoesntAppliesDiscountToFinalPrice()
        {
            var soup = new Product
            {
                ProductId = 1,
                Name = "Soup",
                BasePrice = 0.65m,
                Discounts = new List<Discount>(),
                Promotions = new List<Promotion>()
            };

            var bread = new Product
            {
                ProductId = 2,
                Name = "Bread",
                BasePrice = 0.80m,
                Discounts = new List<Discount>(),
                Promotions = new List<Promotion>
                {
                    new Promotion()
                    {
                        PromotionId = 1,
                        Event = new Event
                        {
                            StartDate = DateTime.MinValue,
                            EndDate = DateTime.MaxValue
                        },
                        Discount =  new Discount
                        {
                            DiscountId = 1,
                            DiscountType = DiscountType.Percentage,
                            DiscountValue = 50,
                            Event = new Event
                            {
                                StartDate = DateTime.MinValue,
                                EndDate = DateTime.MaxValue
                            }
                        },
                        Condition = new PromotionCondition()
                        {
                            RequiredProductId = 1,
                            RequiredQuantity = 2
                        }
                    }
                }
            };

            var basketItems = new List<BasketItem>
            {
                new BasketItem { Product = soup, ProductId = soup.ProductId, Quantity = 1 },
                new BasketItem { Product = bread, ProductId = bread.ProductId, Quantity = 1 }
            };

            var basket = new Basket
            {
                Items = basketItems
            };

            _mockRepo.Setup(r => r.GetBasketAsync()).ReturnsAsync(basket);

            var result = await _service.GetBasketAsync();

            Assert.Equal(1.45m, result.FinalPrice);
        }
    }
}