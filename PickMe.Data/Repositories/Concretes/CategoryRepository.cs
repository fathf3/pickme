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
    }
}
