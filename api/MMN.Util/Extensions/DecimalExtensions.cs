namespace MMN.Util.Extensions
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// Truncates a decimal number with the given number of decimal digits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal Truncate(this decimal value, byte decimals)
        {
            var round = decimal.Round(value, decimals);

            if (value > 0 && round > value)
            {
                return round - new decimal(1, 0, 0, false, decimals);
            }
            else if (value < 0 && round < value)
            {
                return round + new decimal(1, 0, 0, false, decimals);
            }
            else
            {
                return 0;
            }
        }
    }
}
