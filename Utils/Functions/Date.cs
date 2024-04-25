

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
