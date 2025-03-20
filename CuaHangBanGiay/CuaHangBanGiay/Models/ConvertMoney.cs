using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuaHangBanGiay.Models
{
    public class ConvertMoney
    {
        public static string convertMoney(decimal? money)
        {
            int mn = (int)money; //1 025 000
            string rs = "";
            while (mn >= 1000)
            {
                int du = mn % 1000;
                string strDu = du.ToString(); //0  25
                while (strDu.Length != 3)
                {
                    strDu = "0" + strDu;//000   025
                }
                rs = "." + strDu + rs;//.000  1.025.000
                mn /= 1000; //1 025   1
            }
            return mn + rs;
        }
    }
}