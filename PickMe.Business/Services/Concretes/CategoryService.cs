using Microsoft.EntityFrameworkCore;
using PickMe.Business.Services.Abstractions;
using PickMe.Core.Models;
using PickMe.Data.Repositories.Abstracts;

namespace PickMe.Business.Services.Concretes
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            // Aynı isimde bir kategori var mı kontrol et
            var existingCategory = await _categoryRepository.FindAsync(c => c.Name == category.Name);

            if (existingCategory != null)
            {
                throw new Exception("Bu kategori adı zaten mevcut!");
            }

            return await _categoryRepository.AddAsync(category);
        }


        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdWithSurveyAsync(id);
            
            if (category.Surveys.Count() > 0)
            {
                foreach (var survey in category.Surveys)
                {
                    survey.CategoryId = 6;
                }
            }
            if (category != null)
            {
                await _categoryRepository.RemoveAsync(category);
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category> GetCategoryByIdWithSurveyAsync(int id)
        {
            return await _categoryRepository.GetCategoryByIdWithSurveyAsync(id);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
        }
    }
}
