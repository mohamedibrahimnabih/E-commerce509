using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_commerce.Repository
{
    public class CategoryRepositroy : ICategoryRepository
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public void Create(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
        }

        public void Alter(Category category)
        {
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
        }
        public void Delete(Category category)
        {
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
        }

        public IQueryable<Category> Get(Expression<Func<Category, bool>>? filter = null, Expression<Func<Category, object>>[]? includeProps = null, bool tracked = true)
        {
            IQueryable<Category> query = _dbContext.Categories;

            if (includeProps != null)
            {
                foreach (var item in includeProps)
                {
                    query = query.Include(item);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if(!tracked)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public Category? GetOne(Expression<Func<Category, bool>>? filter)
        {
            return Get(filter).FirstOrDefault();
        }
    }
}
