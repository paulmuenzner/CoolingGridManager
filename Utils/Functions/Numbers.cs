


namespace Utility.Functions
{
    public static class Numbers
    {
        public static decimal RoundDecimal(decimal originalValue, int decimalPlaces)
        {
            return Math.Round(originalValue, decimalPlaces);
        }
    }

}
