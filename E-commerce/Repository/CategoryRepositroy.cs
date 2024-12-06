using E_commerce.Data;
using E_commerce.Models;
using System.Linq.Expressions;

namespace E_commerce.Repository
{
    public class CategoryRepositroy
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

        public IQueryable<Category> Get(Expression<Func<Category, bool>>? filter)
        {
            if(filter == null)
            {
                return _dbContext.Categories;
            }

            return _dbContext.Categories.Where(filter);
        }

        public Category? GetOne(Expression<Func<Category, bool>>? filter)
        {
            return Get(filter).FirstOrDefault();
        }
    }
}
