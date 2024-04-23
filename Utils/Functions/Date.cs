


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

}
