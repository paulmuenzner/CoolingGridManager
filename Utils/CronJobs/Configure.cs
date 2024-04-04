using Quartz;

public class QuartzScheduler
{
    private readonly IScheduler _scheduler;

    public QuartzScheduler(IScheduler scheduler)
    {
        _scheduler = scheduler;
    }

    public async Task Start()
    {
        await _scheduler.Start(); // No extra parameter needed
    }
}

// public class QuartzScheduler
// {
//     private readonly IScheduler _scheduler;

//     public QuartzScheduler(IScheduler scheduler)
//     {
//         _scheduler = scheduler;
//     }
//     public async Task Start()
//     {
//         // Start scheduler
//         await _scheduler.Start();

//         // Schedule all the jobs
//         await ScheduleAddBillJob(scheduler);
//         // await ScheduleAnotherJob(scheduler);
//         // Add more job scheduling methods as needed
//     }

//     private static async Task ScheduleAddBillJob(IScheduler scheduler)
//     {
//         // Define the job and tie it to our AddBillJob class
//         var job = JobBuilder.Create<AddBillJob>()
//             .WithIdentity("addBillJob", "billing")
//             .Build();

//         // Trigger the job to run every two seconds
//         var trigger = TriggerBuilder.Create()
//             .WithIdentity("addBillTrigger", "billing")
//             .StartNow()
//             .WithSimpleSchedule(x => x
//                 .WithIntervalInSeconds(2)
//                 .RepeatForever())
//             .Build();

//         // Tell Quartz to schedule the job using our trigger
//         await scheduler.ScheduleJob(job, trigger);
//     }
// }

public class QuartzHostedService : IHostedService
{
    private readonly IScheduler _scheduler;

    public QuartzHostedService(IScheduler scheduler)
    {
        _scheduler = scheduler;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await new QuartzScheduler(_scheduler).Start(); // Create and start Scheduler
        }
        catch (Exception ex)
        {
            // Handle exceptions and log errors
            Console.WriteLine($"Error starting scheduler: {ex.Message}");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _scheduler.Shutdown();
    }
}