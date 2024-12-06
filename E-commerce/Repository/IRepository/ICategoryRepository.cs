using E_commerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace E_commerce.Repository.IRepository
{
    public interface ICategoryRepository
    {
        public void Create(Category category);

        public void Alter(Category category);
        public void Delete(Category category);

        public IQueryable<Category> Get(Expression<Func<Category, bool>>? filter = null, Expression<Func<Category, object>>[]? includeProps = null, bool tracked = true);

        public Category? GetOne(Expression<Func<Category, bool>>? filter);
    }
}
