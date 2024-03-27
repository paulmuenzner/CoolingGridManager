// using System;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.EntityFrameworkCore;
// using Npgsql;

// public static class DatabaseHelper
// {
//     public static void ConfigureDatabase(IServiceCollection services, string connectionString)
//     {
//         if (string.IsNullOrEmpty(connectionString))
//         {
//             throw new InvalidOperationException("Connection string 'DefaultConnection' is missing in appsettings.json.");
//         }

//         try
//         {
//             var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
//             using (var connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString))
//             {
//                 connection.Open();
//                 services.AddDbContext<CoolingGridDBContext>(options =>
//                     options.UseNpgsql(connectionString));
//             }
//         }
//         catch (Exception ex)
//         {
//             throw new InvalidOperationException("Connection failed: " + ex.Message);
//         }
//     }
// }
