using EghalTech.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EghalTech.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .HasColumnType("nVarChar(100)");
            builder.Property(p => p.Brand)
               .HasColumnType("nVarChar(50)");

            builder.HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.CartItems)
                .WithOne(ci => ci.Product)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.WishlistItems)
                .WithOne(wi => wi.Product)
                .HasForeignKey(wi => wi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(ProductData());
        }

        private List<Product> ProductData()
        {
            return new List<Product>
            {
                new Product{Id=1, Name = "Dell XPS 13",Brand = "Dell",Price = 999.99m,StockQuantity = 15,ImageUrl = "dell-xps-13.webp",Description = "13-inch ultrabook with Intel i7 processor.",CategoryId = 1},
                new Product{Id=2, Name = "MacBook Air M2",Brand = "Apple",Price = 1199.99m,StockQuantity = 10,ImageUrl = "macbook-air-m2.webp",Description = "Apple's lightest laptop with M2 chip.",CategoryId = 1},
                new Product{Id=3, Name = "HP Spectre x360",Brand = "HP",Price = 1099.99m,StockQuantity = 12,ImageUrl = "hp-spectre.webp",Description = "Convertible laptop with touch screen.",CategoryId = 1},
                // Smartphones
                new Product{Id=4 ,Name = "iPhone 14 Pro",Brand = "Apple",Price = 999.99m,StockQuantity = 20,ImageUrl = "iphone-14-pro.webp",Description = "Flagship Apple smartphone with A16 Bionic chip.",CategoryId = 2},
                new Product{Id=5 ,Name = "Samsung Galaxy S23",Brand = "Samsung",Price = 899.99m,StockQuantity = 18,ImageUrl = "galaxy-s23.webp",Description = "High-end Android smartphone from Samsung.",CategoryId = 2},
                new Product{Id=6 ,Name = "Google Pixel 7",Brand = "Google",Price = 799.99m,StockQuantity = 14,ImageUrl = "pixel-7.webp",Description = "Clean Android experience with excellent camera.",CategoryId = 2},
                // Accessories
                new Product{Id=7,Name = "Logitech MX Master 3",Brand = "Logitech",Price = 99.99m,StockQuantity = 25,ImageUrl = "mx-master-3.webp",Description = "Premium wireless mouse for productivity.",CategoryId = 3},
                new Product{Id=8,Name = "Keychron K6 Mechanical Keyboard",Brand = "Keychron",Price = 79.99m,StockQuantity = 30,ImageUrl = "keychron-k6.webp",Description = "Compact mechanical keyboard with RGB lighting.",CategoryId = 3},
                new Product{Id=9,Name = "Anker PowerCore 10000",Brand = "Anker",Price = 39.99m,StockQuantity = 50,ImageUrl = "anker-powercore.webp",Description = "Portable charger with 10000mAh capacity.",CategoryId = 3},
                // Audio Devices
                new Product{Id=10,Name = "Sony WH-1000XM5",Brand = "Sony",Price = 349.99m,StockQuantity = 20,ImageUrl = "sony-wh-1000xm5.webp",Description = "Top-rated noise-cancelling headphones.",CategoryId = 4},
                new Product{Id=11,Name = "Apple AirPods Pro 2",Brand = "Apple",Price = 249.99m,StockQuantity = 30,ImageUrl = "airpods-pro-2.webp",Description = "In-ear noise-cancelling earbuds from Apple.",CategoryId = 4},
                new Product{Id=12,Name = "JBL Flip 6 Bluetooth Speaker",Brand = "JBL",Price = 129.99m,StockQuantity = 40,ImageUrl = "jbl-flip-6.webp",Description = "Portable waterproof Bluetooth speaker.",CategoryId = 4}
            };
        }
    }
}
