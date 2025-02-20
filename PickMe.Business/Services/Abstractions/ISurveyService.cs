using PickMe.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PickMe.Business.Services.Abstractions
{
    public interface ISurveyService
    {
        Task<Survey> CreateSurveyAsync(Survey survey);
        Task<Survey> GetSurveyByIdAsync(int id);
        Task<Survey> GetSurveyWithCommentsAndLikeAsync(int surveyId);
        Task<IEnumerable<Survey>> GetUserSurveysAsync(string userId);
        Task<IEnumerable<Survey>> GetActiveSurveysAsync();
        Task<IEnumerable<Survey>> GetAllSurveysAsync();
        Task<IEnumerable<Survey>> GetMostLikedSurveysAsync();
        Task<IEnumerable<Survey>> GetMostCommentedSurveysAsync();
        Task<IEnumerable<Survey>> GetSurveysByCategoryAsync(int categoryId);
        Task<bool> VoteForImageAsync(int surveyId, bool isFirstImage, string userId);
        Task<bool> AddLikeAsync(int surveyId, string userId);
        Task<bool> RemoveLikeAsync(int surveyId, string userId);
        Task<Comment> AddCommentAsync(int surveyId, string userId, string content);
        Task<Report> ReportSurveyAsync(int surveyId, string userId, string reason);
        Task UpdateSurveyAsync(Survey survey);
        Task DeleteSurveyAsync(int id);
        Task ToggleSurveyStatusAsync(int id);


    }
}
