using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Price).IsRequired().HasColumnType("numeric(18,2)").HasColumnName("Product_Price");


                entity.HasData(
                    new Product { Id = 00001, Name = "New Rak", Price = 10000.00m },
                    new Product { Id = 00002, Name = "Computer", Price = 80000.00m },
                    new Product { Id = 00003, Name = "Generator", Price = 100000.00m }
                );

            }
            ); 
            base.OnModelCreating(modelBuilder);
        }

    }
}
