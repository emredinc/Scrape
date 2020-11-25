using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WebScrape.Dal.Context;

namespace WebScrape.Dal.DependencyInjection
{
    public static class DependencyInject
    {
        public static IServiceCollection AddDataAccessDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJobBoardRepository, JobBoardRepository>();
            services.AddScoped<DbContext, ScrapingContext>();
            services.AddDbContext<ScrapingContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("JobScrapeConnection")));
            return services;
        }
    }
}
