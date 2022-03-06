using Microsoft.EntityFrameworkCore;
using NLayerApp.Core.Models;
using System.Reflection;

namespace NLayerApp.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //assembly o class library gibi düşünebilirsin buradada tüm configuration dosyaları aynı interfaceden türediğinden
            //direk olarak çalıştığım assemblyden alabilirsin diyebiliyorsun.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //modelBuilder.ApplyConfiguration(new CategoryConfiguration());

            //buraya yapılması iyi değildir normalde unutma!
            modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature
            {
                Id = 1,
                Color = "Kırmızı",
                Height = 100,
                ProductId = 1,
                Width = 200
            },
            new ProductFeature
            {
                Id = 2,
                Color = "Mavi",
                Height = 100,
                ProductId = 2,
                Width = 200
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
