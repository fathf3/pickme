using Microsoft.AspNetCore.Mvc;
using PickMe.Business.Services.Abstractions;
using PickMe.Business.Services.Concretes;
using PickMe.Core.Models;
using PickMe.Web.Models;
using System.Diagnostics;

namespace PickMe.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISurveyService _surveyService;
        private readonly ICategoryService _categoryService;

        public HomeController(ISurveyService surveyService, ICategoryService categoryService)
        {
            _surveyService = surveyService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string filterType, int? categoryId)
        {
            IEnumerable<Survey> surveys;

            // Kategori seçimi varsa
            if (categoryId.HasValue && categoryId > 0)
            {
                surveys = await _surveyService.GetSurveysByCategoryAsync(categoryId.Value);

                // Kategoriye göre filtrelenen sonuçlara sýralama uygulamak için
                if (filterType != null)
                {
                    switch (filterType)
                    {
                        case "mostLiked":
                            surveys = surveys.OrderByDescending(s => s.Likes.Count).ToList();
                            break;
                        case "mostCommented":
                            surveys = surveys.OrderByDescending(s => s.Comments.Count).ToList();
                            break;
                        default:
                            // Varsayýlan olarak en yeni anketleri sýralar (createdDate'ye göre)
                            surveys = surveys.OrderByDescending(s => s.CreatedAt).ToList();
                            break;
                    }
                }
            }
            // Kategori seçimi yoksa
            else if (filterType != null)
            {
                switch (filterType)
                {
                    case "mostLiked":
                        surveys = await _surveyService.GetMostLikedSurveysAsync();
                        break;
                    case "mostCommented":
                        surveys = await _surveyService.GetMostCommentedSurveysAsync();
                        break;
                    default:
                        surveys = await _surveyService.GetActiveSurveysAsync();
                        break;
                }
            }
            // Ne kategori ne de filtre tipi yoksa
            else
            {
                surveys = await _surveyService.GetActiveSurveysAsync();
            }


            var categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SelectedFilter = filterType;

            return View(surveys);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error(int? code)
        {
            if (code.HasValue)
            {
                ViewBag.StatusCode = code.Value;
            }
            return View();
        }
    }
}
