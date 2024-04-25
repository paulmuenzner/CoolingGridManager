using Microsoft.Extensions.Options;
using NCrontab;
using Quartz;
using Utility.Functions;

namespace CoolingGridManager.Utils.CronJobs
{
    public class CalculateEfficiencySetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            // Calculate cron schedule
            // !! Must be after cron job to calculate grid consumption !! 
            string currentExpression = AppData.CronScheduleCreateBills;
            // Cron job for grid consumption runs at midnight. Here we modify it to 3 am; therefore thre hours later
            string cronScheduleAfterGridConsumptionCron = Cron.ModifyMinHourCronSchedule(currentExpression, "3", "0");

            var schedule = CrontabSchedule.Parse(currentExpression);
            var jobKey = JobKey.Create(nameof(CreateBills));
            options
                .AddJob<CreateBills>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                .AddTrigger(trigger => trigger
                        .ForJob(jobKey)
                        .WithCronSchedule(cronScheduleAfterGridConsumptionCron));
        }
    }
}