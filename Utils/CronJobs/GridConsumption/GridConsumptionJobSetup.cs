using Microsoft.Extensions.Options;
using Quartz;

namespace CoolingGridManager.Utils.CronJobs
{
    public class ValidateBillsSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(GridConsumption));
            options
                .AddJob<GridConsumption>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
            // .WithCronSchedule("0 0 0 3 * ? *"));
            .WithSimpleSchedule(schedule =>
                schedule.WithIntervalInSeconds(5).RepeatForever()));
        }
    }
}