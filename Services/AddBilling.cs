// using System;
// using System.Linq;
// using Microsoft.EntityFrameworkCore;
// using CoolingGridManagementTool.Models;

// namespace CoolingGridTool.Data
// {
//     public class BillingDataService
//     {
//         private readonly YourDbContext _dbContext;

//         public BillingDataService(YourDbContext dbContext)
//         {
//             _dbContext = dbContext;
//         }

//         public void AddBillingData(int consumerId, int billingMonth, int billingYear, decimal totalConsumption, decimal billingAmount)
//         {
//             // Assuming you have a MonthlyBilling DbSet in your DbContext
//             var billingEntry = new MonthlyBilling
//             {
//                 ConsumerID = consumerId,
//                 BillingMonth = billingMonth,
//                 BillingYear = billingYear,
//                 TotalConsumption = totalConsumption,
//                 BillingAmount = billingAmount
//             };

//             // Validate and assign the Consumer
//             ValidateAndAssignConsumer(billingEntry);

//             // Add the billing entry to the database
//             _dbContext.MonthlyBillings.Add(billingEntry);
//             _dbContext.SaveChanges();
//         }

//         private void ValidateAndAssignConsumer(MonthlyBilling billingEntry)
//         {
//             // Ensure that the ConsumerID corresponds to an existing Consumer in the database
//             if (_dbContext.Consumers.Any(c => c.ConsumerID == billingEntry.ConsumerID))
//             {
//                 // If validation succeeds, set the Consumer property
//                 billingEntry.Consumer = _dbContext.Consumers.Single(c => c.ConsumerID == billingEntry.ConsumerID);
//             }
//             else
//             {
//                 // If validation fails, you can throw an exception or handle it according to your logic
//                 throw new InvalidOperationException("Consumer validation failed.");
//             }
//         }
//     }
// }