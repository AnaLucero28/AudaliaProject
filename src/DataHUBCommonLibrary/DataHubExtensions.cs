using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audalia.DataHUBCommon
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this object value)
        {
            DateTime result = DateTime.MinValue;
            if (value != DBNull.Value) //value.ToString() != "")
                result = DateTime.ParseExact(value.ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            return result;
        }

        public static DateTime? ToNullableDateTime(this object value)
        {
            DateTime? result = null;
            if (value.ToString() != "")
                result = DateTime.ParseExact(value.ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            return result;
        }

        public static object ToDatabaseValue(this DateTime? date)
        {
            object result = null;
            if (date != null)
                result = date.Value.ToString("yyyyMMddHHmmss");
            else
                result = DBNull.Value;

            return result;
        }

        public static string ToShortDateString(this DateTime? date)
        {
            string result = "";
            if (date != null)
                result = date.Value.ToShortDateString();
            
            return result;
        }

        public static string ToDatabaseString(this DateTime date)
        {
            string result = "";
            result = date.ToString("yyyyMMddHHmmss");

            return result;
        }


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

        public static Color StringToColor(this string colorStr)
        {
            Color result = Color.Transparent;           
            if (!string.IsNullOrWhiteSpace(colorStr))
            {
                result = ColorTranslator.FromHtml(colorStr);                
            }
            return result;
        }

        public static DateTime ToDateTime(this string str)
        {
            var result = DateTime.ParseExact(str, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            return result;
        }

    }


    public static class ColorExtensions
    {
        public static string ColorToString(this Color color)
        {            
            string result = ColorTranslator.ToHtml(color);
            return result;
        }

    }

}
