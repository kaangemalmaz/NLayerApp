using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayerApp.Core.Models;

namespace NLayerApp.Repository.Seeds
{
    internal class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product
                {
                    Id = 1,
                    CategoryId = 1,
                    Price = 100,
                    Stock = 20,
                    CreatedTime = DateTime.Now,
                    Name = "Kalem 1"
                },
                new Product
                {
                    Id = 2,
                    CategoryId = 1,
                    Price = 100,
                    Stock = 20,
                    CreatedTime = DateTime.Now,
                    Name = "Kalem 2"
                },
                new Product
                {
                    Id = 3,
                    CategoryId = 1,
                    Price = 100,
                    Stock = 20,
                    CreatedTime = DateTime.Now,
                    Name = "Kalem 3"
                },
                new Product
                {
                    Id = 4,
                    CategoryId = 2,
                    Price = 200,
                    Stock = 40,
                    CreatedTime = DateTime.Now,
                    Name = "Fen Kitabı"
                },
                new Product
                {
                    Id = 5,
                    CategoryId = 3,
                    Price = 300,
                    Stock = 60,
                    CreatedTime = DateTime.Now,
                    Name = "Yazı Defteri"
                });
        }
    }
}
