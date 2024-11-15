using E_commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=E-Commerce509;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasData(new Category()
                {
                    Id = 1,
                    Name = "Mobiles"
                });

            modelBuilder.Entity<Category>()
                .HasData(new Category()
                {
                    Id = 2,
                    Name = "Laptops"
                });

            modelBuilder.Entity<Category>()
                .HasData(new Category()
                {
                    Id = 3,
                    Name = "Tablets"
                });

            modelBuilder.Entity<Product>()
                .HasData(new Product()
                {
                    Id = 1,
                    Name = "Iphone 13",
                    Description = "This is iphone 13",
                    Price = 40000.00m,
                    Img = "1.png",
                    Quantity = 5,
                    Rate = 0,
                    CategoryId = 1
                });

            modelBuilder.Entity<Product>()
                .HasData(new Product()
                {
                    Id = 2,
                    Name = "Iphone 14",
                    Description = "This is iphone 14",
                    Price = 50000.00m,
                    Img = "2.png",
                    Quantity = 6,
                    Rate = 0,
                    CategoryId = 1
                });

            modelBuilder.Entity<Product>()
                .HasData(new Product()
                {
                    Id = 3,
                    Name = "Iphone 15",
                    Description = "This is iphone 15",
                    Price = 60000.00m,
                    Img = "3.png",
                    Quantity = 7,
                    Rate = 0,
                    CategoryId = 1
                });
        }
    }
}
