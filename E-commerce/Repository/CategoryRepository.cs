using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repository.IRepository;

namespace E_commerce.Repository
{
    public class CategoryRepository : Repositroy<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
