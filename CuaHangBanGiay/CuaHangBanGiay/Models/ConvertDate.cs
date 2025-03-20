using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuaHangBanGiay.Models
{
    public class ConvertDate
    {
        public static string getStrDate(DateTime? date)
        {
            DateTime datetime = (DateTime)date;
            string day = datetime.Day.ToString();
            if (datetime.Day < 10)
            {
                day = "0" + day;
            }
            string month = datetime.Month.ToString();
            if (datetime.Month < 10)
            {
                month = "0" + month;
            }
            string year = datetime.Year.ToString();
            if (datetime.Year < 1000)
            {
                year = "0" + year;
            }
            if (datetime.Year < 100)
            {
                year = "0" + year;
            }
            if (datetime.Year < 10)
            {
                year = "0" + year;
            }
            return year + "-" + month + "-" + day;
        }
    }
}