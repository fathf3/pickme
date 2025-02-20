using Microsoft.EntityFrameworkCore;
using PickMe.Core.Models;
using PickMe.Data.Repositories.Abstracts;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PickMe.Data.Repositories.Concretes
{
    public class SurveyRepository : Repository<Survey>, ISurveyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SurveyRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Survey>> GetUserSurveysAsync(string userId)
        {
            return await _dbContext.Surveys
                .Include(s => s.Comments)
                .Include(s => s.Likes)
                .Include(s => s.Reports)
                .Include(s => s.Category)
                .Where(s => s.CreatedById == userId)
                .ToListAsync();
        }
        public async Task<Survey> GetSurveyWithCommentsAndLikeAsync(int surveyId)
        {
            return await _dbContext.Surveys
                .Include(s => s.Comments)
                .ThenInclude(c => c.User)
                .Include(s => s.Likes)
                .Include(s => s.Reports)
                .Include(s => s.CreatedBy)
                .FirstOrDefaultAsync(s => s.Id == surveyId);

        }

        public async Task<IEnumerable<Survey>> GetActiveSurveysAsync()
        {
            var x = await _dbContext.Surveys
                .Include(s => s.Likes)
                .Include(s => s.Reports)
                .Include(s => s.Comments)
                .Include(s => s.CreatedBy)
                .Include(s => s.Category)
                .Where(s => s.IsActive)
                
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            return x;
        }

        public async Task<bool> HasUserVotedAsync(int surveyId, string userId)
        {
            return await _dbContext.Votes
                .AnyAsync(v => v.SurveyId == surveyId && v.UserId == userId);
        }

        public async Task IncrementVoteAsync(int surveyId, bool isFirstImage)
        {
            var survey = await _dbContext.Surveys.FindAsync(surveyId);
            if (survey != null)
            {
                if (isFirstImage)
                    survey.Image1Votes++;
                else
                    survey.Image2Votes++;

                await SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Survey>> GetSurveysByCategoryAsync(int categoryId)
        {
            var x = await _dbContext.Surveys
                .Include(s => s.Likes)
                .Include(s => s.Reports)
                .Include(s => s.Comments)
                .Include(s => s.CreatedBy)
                .Include(s => s.Category)
                .Where(s => s.CategoryId == categoryId && s.IsActive)
                .ToListAsync();
                
            return x;
        }

        public async Task<IEnumerable<Survey>> GetAllSurveysAsync()
        {
            var x = await _dbContext.Surveys
                 .Include(s => s.Likes)
                 .Include(s => s.Reports)
                 .Include(s => s.Comments)
                 .Include(s => s.CreatedBy)
                 .Include(s => s.Category)
                 .ToListAsync();

            return x;
        }
    }
}
