using KantarAssessment.ShoppingBasketAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace KantarAssessment.ShoppingBasketAPI.Data
{
    public class ShoppingContext : DbContext
    {
        public ShoppingContext(DbContextOptions<ShoppingContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Basket>()
                .HasMany(b => b.Items)
                .WithOne()
                .HasForeignKey(bi => bi.BasketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BasketItem>()
                .HasKey(bi => new { bi.BasketId, bi.ProductId });

            modelBuilder.Entity<BasketItem>()
                .HasOne(bi => bi.Product)
                .WithMany()
                .HasForeignKey(bi => bi.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Discounts)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Promotions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Discount>()
                .HasOne(d => d.Event)
                .WithMany()   
                .HasForeignKey("EventId")  
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Promotion>()
                .HasOne(p => p.Event)
                .WithMany()
                .HasForeignKey("EventId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Promotion>()
                .HasOne(p => p.Discount)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Promotion>()
                .OwnsOne(p => p.Condition);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OrderId);

                entity.Property(o => o.OrderDate)
                    .IsRequired();

                entity.HasMany(o => o.Items)
                    .WithOne(i => i.Order)
                    .HasForeignKey(i => i.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(i => i.OrderItemId);

                entity.Property(i => i.ProductName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(i => i.UnitBasePrice)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                entity.Property(i => i.UnitFinalPrice)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                entity.Property(i => i.Quantity)
                      .IsRequired();
            });
        }
    }
}