using Microsoft.AspNetCore.Mvc;
using PickMe.Business.Services.Abstractions;
using PickMe.Core.Models;
using PickMe.Web.Models;
using System.Diagnostics;

namespace PickMe.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISurveyService _surveyService;

        public HomeController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        public async Task<IActionResult> Index(string filterType)
        {
            IEnumerable<Survey> surveys;

            switch (filterType)
            {
                case "mostLiked":
                    surveys = await _surveyService.GetMostLikedSurveysAsync();
                    break;
                case "mostCommented":
                    surveys = await _surveyService.GetMostCommentedSurveysAsync();
                    break;

                default: // En yeni anketler (varsayýlan)
                    surveys = await _surveyService.GetActiveSurveysAsync();
                    break;
            }

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
