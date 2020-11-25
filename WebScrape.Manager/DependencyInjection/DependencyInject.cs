using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WebScrape.Dal.DependencyInjection;

namespace WebScrape.Manager.DependencyInjection
{
    public static class DependencyInject
    {
        public static IServiceCollection AddBusinessLayerDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJobBoardService, JobBoardManager>();
            services.AddDataAccessDI(configuration);
            return services;
        }
    }
}
