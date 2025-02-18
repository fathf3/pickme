using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PickMe.Business.Services.Abstractions;
using PickMe.Core.Models;
using PickMe.Core.ViewModels;
using System.Threading.Tasks;

namespace PickMe.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    
                    
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanýcýyý önce username olarak ara, yoksa email olarak ara
                var user = await _userManager.FindByNameAsync(model.UserName) ??
                           await _userManager.FindByEmailAsync(model.UserName);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Güvenlik açýsýndan eðer kullanýcý yoksa bile "E-posta gönderildi" mesajýný göster.
                    return View("ForgotPasswordConfirmation");
                }

                // Þifre sýfýrlama token oluþtur
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Kullanýcýya gidecek þifre sýfýrlama baðlantýsý
                var resetLink = Url.Action("ResetPassword", "Account",
                    new { token, email = user.Email }, Request.Scheme);

                // Burada e-posta gönderme servisini kullanarak kullanýcýya mail göndermelisin.
                await _emailService.SendEmailAsync(user.Email, "Þifre Sýfýrlama Talebi",
                    $"Þifrenizi sýfýrlamak için <a href='{resetLink}'>buraya týklayýn</a>.", true);

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            // Eðer token geçersizse veya daha önce kullanýlmýþsa
            foreach (var error in result.Errors)
            {
                if (error.Code == "InvalidToken")
                {
                    ModelState.AddModelError(string.Empty, "Bu þifre sýfýrlama baðlantýsý zaten kullanýlmýþ veya süresi dolmuþ. Lütfen tekrar þifre sýfýrlama isteðinde bulunun.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}
