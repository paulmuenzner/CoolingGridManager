// Helpers.cs
namespace Utility.ValidatorHelpers
{
    public static class DateHelpers
    {
        public static bool HaveSameMonth(DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            // Extract month and year components from the start and end dates
            var startMonthYear = new DateTime(dateTimeStart.Year, dateTimeStart.Month, 1);
            var endMonthYear = new DateTime(dateTimeEnd.Year, dateTimeEnd.Month, 1);

            // Compare if the month and year components are the same
            return startMonthYear == endMonthYear;
        }
    }

    public static class TypeCheck
    {
        public static bool BeAValidPrecision(decimal value, int maxDecimalPlaces)
        {
            // Convert the decimal to a string
            var valueAsString = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            // Check if the string contains a decimal point
            if (valueAsString.Contains('.'))
            {
                // Get the number of digits after the decimal point
                var digitsAfterDecimalPoint = valueAsString.Split('.')[1].Length;

                // Return true if there are the maximum allowed or fewer digits after the decimal point, otherwise false
                return digitsAfterDecimalPoint <= maxDecimalPlaces;
            }

            return true;
        }
    }
}
