using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PickMe.Business.Services.Abstractions;
using PickMe.Business.Services.Concretes;
using PickMe.Core.Models;
using PickMe.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickMe.Business
{
    public static class ServiceRegistration
    {
        public static void AddBusinessService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISurveyService, SurveyService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICategoryService, CategoryService>();
            

            //services.Configure<EmailSettingsViewModel>(configuration.GetSection("Email"));
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(15); // Token geçerlilik süresi 15 dakika olacak
            });
        }
    }
}