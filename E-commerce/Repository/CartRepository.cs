using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repository.IRepository;

namespace E_commerce.Repository
{
    public class CartRepository : Repositroy<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
