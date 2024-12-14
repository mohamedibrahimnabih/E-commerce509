using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repository.IRepository;

namespace E_commerce.Repository
{
    public class RequestCompnayRepository : Repositroy<RequestCompnay>, IRequestCompnayRepository
    {
        public RequestCompnayRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
