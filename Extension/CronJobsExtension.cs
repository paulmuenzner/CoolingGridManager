using Quartz;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure;

namespace CoolingGridManager.Extensions
{

    public static class CronJobExtension
    {
        public static void AddCustomCronJobs(this IServiceCollection services)
        {
            services.AddQuartz(options => { });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            services.ConfigureOptions<LoggingBackgroundJobSetup>();
        }
    }
}
