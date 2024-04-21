using Microsoft.Extensions.Options;
using Quartz;

namespace CoolingGridManager.Utils.CronJobs
{
    public class AddBillsSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(CreateBills));
            options
                .AddJob<CreateBills>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInSeconds(3).RepeatForever()));
        }
    }
}