using Quartz;
using CoolingGridManager.Utils.CronJobs;


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

            services.ConfigureOptions<AddBillsSetup>();
        }
    }
}
