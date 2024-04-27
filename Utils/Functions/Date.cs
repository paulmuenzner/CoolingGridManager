

namespace Utility.Functions
{
    public static class Date
    {
        public static int TimeSpanInSeconds(DateTime dateStart, DateTime dateEnd)
        {
            // Calculate the time difference
            TimeSpan timeDifference = dateEnd - dateStart;

            // Convert the time difference to seconds
            int timeSpanSeconds = (int)timeDifference.TotalSeconds;

            return timeSpanSeconds;

        }

        // GET START DATETIME AND END DATETIME BY MONTH AND YEAR
        public static (DateTimeOffset startDate, DateTimeOffset endDate) GetStartEndDateTime(int month, int year)
        {
            DateTimeOffset startDate = new DateTimeOffset(year, month, 1, 0, 0, 0, TimeSpan.Zero);
            DateTimeOffset endDate = startDate.AddMonths(1).AddTicks(-1);

            return (startDate, endDate);
        }
    }

    public static class Cron
    {
        public static string ModifyMinHourCronSchedule(string currentExpression, string hour, string minute)
        {

            // Split the expression into components
            var expressionParts = currentExpression.Split(' ');

            // Modify minute and hour fields
            expressionParts[0] = minute;
            expressionParts[1] = hour;

            // Join the modified expression parts back together
            return string.Join(" ", expressionParts);
        }
    }

}
