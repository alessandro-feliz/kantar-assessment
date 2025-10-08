using KantarAssessment.ShoppingBasketAPI.Model;

namespace KantarAssessment.ShoppingBasketAPI.Data.Helpers
{
    public static class DatabaseSeeder
    {
        public static void SeedDevData(ShoppingContext context)
        {
            SeedProducts(context);
            SeedDiscounts(context);
            SeedPromotions(context);
        }

        private static void SeedProducts(ShoppingContext context)
        {
            if (!context.Products.Any())
            {
                var now = DateTime.UtcNow;

                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "Soup",
                        Description = "Tin of soup",
                        BasePrice = 0.65m,
                        ImageUrl = "~/Uploads/soup.jpeg",
                        CreatedAt = now
                    },
                    new Product
                    {
                        Name = "Bread",
                        Description = "Loaf of bread",
                        BasePrice = 0.80m,
                        ImageUrl = "~/Uploads/bread.png",
                        CreatedAt = now
                    },
                    new Product
                    {
                        Name = "Milk",
                        Description = "1L bottle of milk",
                        BasePrice = 1.30m,
                        ImageUrl = "~/Uploads/milk.jpeg",
                        CreatedAt = now
                    },
                    new Product
                    {
                        Name = "Apples",
                        Description = "Bag of apples",
                        BasePrice = 1.00m,
                        ImageUrl = "~/Uploads/apples.jpg",
                        CreatedAt = now
                    }
                };

                context.Products.AddRange(products);
                context.SaveChanges();

                Console.WriteLine("Seeded test product data.");
            }
        }

        private static void SeedDiscounts(ShoppingContext context)
        {
            var applesProduct = context.Products.FirstOrDefault(p => p.Name == "Apples");

            if (applesProduct != null && (applesProduct.Discounts == null || !applesProduct.Discounts.Any(d => d.Event?.Description == "10% off Apples")))
            {
                var now = DateTime.UtcNow;

                var discount = new Discount
                {
                    DiscountType = DiscountType.Percentage,
                    DiscountValue = 10,
                    Event = new Event
                    {
                        Description = "10% off Apples",
                        StartDate = now,
                        EndDate = now.AddDays(30)
                    }
                };

                applesProduct.Discounts ??= new List<Discount>();
                applesProduct.Discounts.Add(discount);

                context.SaveChanges();
            }
        }

        private static void SeedPromotions(ShoppingContext context)
        {
            var soup = context.Products.FirstOrDefault(p => p.Name == "Soup");
            var bread = context.Products.FirstOrDefault(p => p.Name == "Bread");

            if (soup == null || bread == null)
                return;

            var existingPromotion = context.Set<Promotion>()
                .FirstOrDefault(p => p.Event.Description == "Buy 2 soups, get bread 50% off");

            if (existingPromotion != null)
                return;

            var now = DateTime.UtcNow;

            var promotion = new Promotion
            {
                Event = new Event
                {
                    Description = "Buy 2 soups, get bread 50% off",
                    StartDate = now,
                    EndDate = now.AddDays(15)
                },
                Condition = new PromotionCondition
                {
                    RequiredProductId = soup.ProductId,
                    RequiredQuantity = 2
                },
                Discount = new Discount
                {
                    DiscountType = DiscountType.Percentage,
                    DiscountValue = 50,
                    Event = new Event
                    {
                        Description = "Buy 2 soups, get bread 50% off",
                        StartDate = now,
                        EndDate = now.AddDays(15)
                    },
                }
            };

            bread.Promotions ??= new List<Promotion>();
            bread.Promotions.Add(promotion);

            context.SaveChanges();
        }
    }
}