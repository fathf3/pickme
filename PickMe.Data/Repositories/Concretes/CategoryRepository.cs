using Microsoft.EntityFrameworkCore;
using PickMe.Core.Models;
using PickMe.Data.Repositories.Abstracts;

namespace PickMe.Data.Repositories.Concretes
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Category> GetCategoryByIdWithSurveyAsync(int id)
        {
            var category = await _dbContext.Categories
                .Include(c => c.Surveys)
                .FirstOrDefaultAsync(c => c.Id == id);
            return category;
        }
    }
}
