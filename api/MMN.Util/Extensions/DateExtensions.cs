using System;

namespace MMN.Util.Extensions
{
    public static class DateExtensions
    {
        private static readonly TimeZoneInfo BrasiliaTimeZone = CreateBrasiliaTimeZone();

        private static TimeZoneInfo CreateBrasiliaTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            }
            catch
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                }
                catch
                {
                    return TimeZoneInfo.CreateCustomTimeZone("BRT", TimeSpan.FromHours(-3), "Brasilia Time", "Brasilia Standard Time");
                }
            }
        }

        public static DateTime HorarioBrasilia(this DateTime data)
        {
            return TimeZoneInfo.ConvertTime(data, BrasiliaTimeZone);
        }
    }
}
