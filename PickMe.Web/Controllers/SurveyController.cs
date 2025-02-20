using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMe.Core.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using PickMe.Business.Services.Abstractions;
using PickMe.Core.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PickMe.Web.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        private readonly ISurveyService _surveyService;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;

        public SurveyController(ISurveyService surveyService, IImageService imageService, IUserService userService, ICategoryService categoryService)
        {
            _surveyService = surveyService;
            _imageService = imageService;
            _userService = userService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoryAsync();

        var categories2 = categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
        .ToList();

            var model = new SurveyViewModel { Categories = categories2 };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SurveyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoryAsync();
                model.Categories = categories
           .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
           .ToList();
                return View(model);
            }

            var userId = _userService.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Resim dosyalarını kaydet
            string image1Path = await _imageService.UploadSurveyImageAsync(model.Image1File);
            string image2Path = await _imageService.UploadSurveyImageAsync(model.Image2File);

            var survey = new Survey
            {
                Title = model.Title,
                Description = model.Description,
                CreatedById = userId,
                Image1Url = image1Path,
                Image2Url = image2Path,
                CategoryId = model.CategoryId
            };

            await _surveyService.CreateSurveyAsync(survey);
            return RedirectToAction("Index","Home");
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var survey = await _surveyService.GetSurveyWithCommentsAndLikeAsync(id);
            if (survey == null)
            {
                return NotFound();
            }
            return View(survey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vote(int surveyId, bool isFirstImage)
        {
            var userId = _userService.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _surveyService.VoteForImageAsync(surveyId, isFirstImage, userId);
            if (result)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Bu ankete daha önce oy kullandınız." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int surveyId, string content)
        {
            var userId = _userService.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var comment = await _surveyService.AddCommentAsync(surveyId, userId, content);
            return RedirectToAction("Details", new { id = surveyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLike(int surveyId)
        {
            var userId = _userService.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _surveyService.RemoveLikeAsync(surveyId, userId);
            if (!result)
            {
                result = await _surveyService.AddLikeAsync(surveyId, userId);
            }
            // Buton işlevinden sonra aynı sayfada kalır.
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(int surveyId, string reason)
        {
            var userId = _userService.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var report = await _surveyService.ReportSurveyAsync(surveyId, userId, reason);
            TempData["ReportSuccess"] = "Şikayetiniz başarıyla gönderildi."; // Mesajı sakla
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userService.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var survey = await _surveyService.GetSurveyByIdAsync(id);
            if (survey == null || survey.CreatedById != userId)
            {
                return NotFound();
            }

            return View(survey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Survey survey)
        {
            var userId = _userService.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var existingSurvey = await _surveyService.GetSurveyByIdAsync(survey.Id);
                if (existingSurvey == null || existingSurvey.CreatedById != userId)
                {
                    return NotFound();
                }

                await _surveyService.UpdateSurveyAsync(survey);
                return RedirectToAction(nameof(Details), new { id = survey.Id });
            }

            return View(survey);
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userService.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var survey = await _surveyService.GetSurveyByIdAsync(id);
            if (survey == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin") || survey.CreatedById.Equals(userId))
            {
                // Survey'i silme işlemi burada yapılabilir
                await _surveyService.DeleteSurveyAsync(id);
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                return Forbid();
            }

            
        }

        // GET: Survey
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            var surveys = await _surveyService.GetAllSurveysAsync();

            return View(surveys);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            await _surveyService.ToggleSurveyStatusAsync(id);
            return RedirectToAction("AdminIndex");
        }

        [HttpGet]
        public async Task<IActionResult> Comments(int id)
        {
            var comments = await _surveyService.GetSurveyWithCommentsAndLikeAsync(id);

            return View(comments);
        }

        
        [HttpGet]
        public async Task<IActionResult> MySurvey()
        {
            var userId = _userService.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var surveys = await _surveyService.GetUserSurveysAsync(userId);
            return View(surveys);
        }

    }
}
