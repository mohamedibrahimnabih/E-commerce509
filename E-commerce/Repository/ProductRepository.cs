using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repository.IRepository;

namespace E_commerce.Repository
{
    public class ProductRepository : Repositroy<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
