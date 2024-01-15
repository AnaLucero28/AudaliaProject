using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audalia.DataHUBClient
{
    /*
    public static class DateTimeExtensions
    {
        public static int GetIso8601WeekOfYear(this DateTime time)
        {
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime TruncStartDate(this DateTime dateTime)
        {
            var result = dateTime.Date;

            return result;
        }
        public static DateTime TruncFinishDate(this DateTime dateTime)
        {
            var result = dateTime.Date;
            if ((dateTime - result).Ticks != 0)
                result = result.AddDays(1);

            return result;
        }

        public static DateTime RoundDate(this DateTime dateTime)
        {
            var result = dateTime.Date;
            if (dateTime.Hour > 12)
                result = result.AddDays(1);
            
            return result;
        }        
    }

    public static class StringExtensions
    {
        public static String Capitalize(this String text)
        {
            string result = "";
            if (!string.IsNullOrEmpty(text))
            {                
                char[] charArray = text.ToCharArray();
                charArray[0] = (text.ToUpperInvariant())[0]; 
                
                result = new string(charArray);
            }
            return result;
        }
    }
    */
}
