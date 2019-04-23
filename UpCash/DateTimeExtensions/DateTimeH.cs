using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTimeH
{
    public static class DateTimeHelper
    {
        public static string GetDateNow(char separator)
        {
            DateTime dateTime = DateTime.Now;
            return dateTime.Day.ToString() + separator + dateTime.Month.ToString() + separator + dateTime.Year.ToString();
        }
    }
}
