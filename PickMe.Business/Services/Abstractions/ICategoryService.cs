using PickMe.Core.Models;

namespace PickMe.Business.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoryAsync();
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Category> GetCategoryByIdWithSurveyAsync(int id);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
}
