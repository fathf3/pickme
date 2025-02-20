using PickMe.Core.Models;

namespace PickMe.Data.Repositories.Abstracts
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetCategoryByIdWithSurveyAsync(int id);
    }
}