using Microsoft.Extensions.DependencyInjection;
using PickMe.Data.Repositories.Abstracts;
using PickMe.Data.Repositories.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickMe.Data
{
    public static class ServiceRegistration
    {
        public static void AddDataService(this IServiceCollection services)
        {
            services.AddScoped<ISurveyRepository, SurveyRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

        }
    }
}
