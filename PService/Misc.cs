using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PService
{
    public class Misc
    {
        public static float TrueFloat(float value) => float.Parse(value.ToString());

        public static decimal GetTruePrice(decimal value)
        {
            decimal a = decimal.Parse(value.ToString());

            string trimmed = a.ToString("#.000");

            return decimal.Parse(trimmed.ToString());
        } 

        public static int GetTodayMonth()
        {
            string sMonth = DateTime.Now.ToString("MM");

            return int.Parse(sMonth);
        }
    }
}
