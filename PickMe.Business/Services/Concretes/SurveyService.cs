using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PickMe.Business.Services.Abstractions;
using PickMe.Core.Models;
using PickMe.Data;
using PickMe.Data.Repositories.Abstracts;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PickMe.Business.Services.Concretes
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SurveyService(ISurveyRepository surveyRepository, ApplicationDbContext context, ICommentRepository commentRepository, IWebHostEnvironment environment)
        {
            _surveyRepository = surveyRepository;
            _context = context;
            _commentRepository = commentRepository;
            _environment = environment;
        }

        public async Task<Survey> CreateSurveyAsync(Survey survey)
        {
            survey.CreatedAt = DateTime.UtcNow;
            survey.IsActive = false;
            return await _surveyRepository.AddAsync(survey);
        }

        public async Task<Survey> GetSurveyByIdAsync(int id)
        {
            return await _surveyRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Survey>> GetUserSurveysAsync(string userId)
        {
            return await _surveyRepository.GetUserSurveysAsync(userId);
        }

        public async Task<IEnumerable<Survey>> GetActiveSurveysAsync()
        {
            return await _surveyRepository.GetActiveSurveysAsync();
        }

        public async Task<bool> VoteForImageAsync(int surveyId, bool isFirstImage, string userId)
        {
            if (await _surveyRepository.HasUserVotedAsync(surveyId, userId))
                return false;

            var vote = new Vote
            {
                SurveyId = surveyId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Votes.AddAsync(vote);
            await _surveyRepository.IncrementVoteAsync(surveyId, isFirstImage);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddLikeAsync(int surveyId, string userId)
        {
            try
            {
                var like = new Like
                {
                    SurveyId = surveyId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Likes.AddAsync(like);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveLikeAsync(int surveyId, string userId)
        {
            try
            {
                var like = await _context.Likes
                    .FirstOrDefaultAsync(l => l.SurveyId == surveyId && l.UserId == userId);

                if (like != null)
                {
                    _context.Likes.Remove(like);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Comment> AddCommentAsync(int surveyId, string userId, string content)
        {
            var comment = new Comment
            {
                SurveyId = surveyId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Report> ReportSurveyAsync(int surveyId, string userId, string reason)
        {
            var report = new Report
            {
                SurveyId = surveyId,
                UserId = userId,
                Reason = reason,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task UpdateSurveyAsync(Survey survey)
        {
            await _surveyRepository.UpdateAsync(survey);
        }

        public async Task DeleteSurveyAsync(int id)
        {
            var survey = await _surveyRepository.GetByIdAsync(id);
            if (survey != null)
            {

                DeleteImageIfLocal(survey.Image1Url);
                DeleteImageIfLocal(survey.Image2Url);
                await _surveyRepository.RemoveAsync(survey);
            }
        }

        public async Task<bool> HasUserVotedAsync(int surveyId, string userId)
        {
            return await _surveyRepository.HasUserVotedAsync(surveyId, userId);
        }

        public async Task<Survey> GetSurveyWithCommentsAndLikeAsync(int surveyId)
        {
            var x = await _surveyRepository.GetSurveyWithCommentsAndLikeAsync(surveyId);
            return x;
        }

        public async Task<IEnumerable<Survey>> GetMostLikedSurveysAsync()
        {
            var x = await _surveyRepository.GetActiveSurveysAsync();
            var d = x.OrderByDescending(s => s.Likes.Count).ToList();
            return d;

        }
        public async Task<IEnumerable<Survey>> GetMostCommentedSurveysAsync()
        {
            var x = await _surveyRepository.GetActiveSurveysAsync();
            var d = x.OrderByDescending(s => s.Comments.Count).ToList();
            return d;

        }

        public async Task<IEnumerable<Survey>> GetSurveysByCategoryAsync(int categoryId)
        {
            return await _surveyRepository.GetSurveysByCategoryAsync(categoryId);
        }

        public Task<IEnumerable<Survey>> GetAllSurveysAsync()
        {
            return _surveyRepository.GetAllSurveysAsync();
        }

        public async Task ToggleSurveyStatusAsync(int id)
        {
            var survey = await _surveyRepository.GetByIdAsync(id);
            if (survey != null)
            {
                survey.IsActive = !survey.IsActive;
                await _surveyRepository.UpdateAsync(survey);
            }
        }
    
    private void DeleteImageIfLocal(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var webRootPath = _environment.WebRootPath;
            var filePath = Path.Combine(webRootPath, imageUrl.TrimStart('/'));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
