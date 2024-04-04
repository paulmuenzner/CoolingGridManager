using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;
using CoolingGridManager.Services;
using CoolingGridManager.Models.Data;

public class AddBillJob : IJob
{
    private readonly BillingService _billingService;

    public AddBillJob(BillingService billingService)
    {
        _billingService = billingService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var random = new Random();
            var doubles = random.NextDouble() * 1;
            // Generate arbitrary values for Billing
            var bill = new Billing
            {
                ConsumerID = random.Next(1, 100), // Assuming you have consumers with IDs 1-100
                BillingMonth = random.Next(1, 12),
                BillingYear = DateTime.Now.Year,
                TotalConsumption = (decimal)(doubles * 100.025), // Arbitrary value
                IsPaid = false,
                BillingAmount = (decimal)(doubles * 1.5) // Arbitrary value
            };


            // Call AddBill method with generated Billing object
            await _billingService.AddBill(bill);
            Console.WriteLine("Bill added successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add bill: {ex.Message}");
        }
    }
}