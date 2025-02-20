using PickMe.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PickMe.Data.Repositories.Abstracts
{
    public interface ISurveyRepository : IRepository<Survey>
    {
        Task<IEnumerable<Survey>> GetUserSurveysAsync(string userId);
        Task<IEnumerable<Survey>> GetActiveSurveysAsync();
        Task<IEnumerable<Survey>> GetAllSurveysAsync();
        Task<bool> HasUserVotedAsync(int surveyId, string userId);
        Task IncrementVoteAsync(int surveyId, bool isFirstImage);
        Task<Survey> GetSurveyWithCommentsAndLikeAsync(int surveyId);
        Task<IEnumerable<Survey>> GetSurveysByCategoryAsync(int categoryId);


    }
}
